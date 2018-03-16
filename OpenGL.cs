using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;

using static CSGL.CSGL;
using static CSGL.OpenGL;
using static RGL.Buffer;

namespace RGL
{

    #region VertexArray

    public enum DataType : uint
    {
        Byte = GL_BYTE,
        UnsignedByte = GL_UNSIGNED_BYTE,
        Short = GL_SHORT,
        UnsignedShort = GL_UNSIGNED_SHORT,
        Int = GL_INT,
        UnsignedInt = GL_UNSIGNED_INT,
        Float = GL_FLOAT,
        Double = GL_DOUBLE
    }

    public enum DrawMode : uint
    {
        Triangles = GL_TRIANGLES,
        Quads = GL_QUADS,
        Lines = GL_LINES,
        Points = GL_POINTS,
        TriangleStrip = GL_TRIANGLE_STRIP,
        TriangleFan = GL_TRIANGLE_FAN
    }

    public class VertexArray
    {
        private static VertexArray bound;
        private static List<uint> attribArrays;

        static VertexArray()
        {
            bound = null;
            attribArrays = new List<uint>();
        }

        internal uint handle;

        Dictionary<uint, ArrayBuffer> arrayBuffers;
        ElementArrayBuffer elementBuffer;

        public VertexArray()
        {
            glGenVertexArrays(1, ref handle);

            arrayBuffers = new Dictionary<uint, ArrayBuffer>();
            elementBuffer = null;
        }

        ~VertexArray()
        {
            unbind();
            arrayBuffers.Clear();
            elementBuffer = null;
            glDeleteVertexArrays(1, ref handle);
        }

        public void bind()
        {
            if (bound != this)
            {
                glBindVertexArray(handle);
                bound = this;
            }
        }

        public void unbind()
        {
            if (bound != null)
            {
                glBindVertexArray(0);
                bound = null;
            }
        }

        public ElementArrayBuffer addElementBuffer<T>(T[] data, BufferUsage usage = BufferUsage.StaticDraw) where T : struct
        {
            bind();
            ElementArrayBuffer buffer = elementBuffer ?? (elementBuffer = new ElementArrayBuffer());
            buffer.setData(data, usage);

            return buffer;
        }

        public ArrayBuffer addArrayBuffer<T>(uint index, T[] data, DataType type, bool normalized = false, BufferUsage usage = BufferUsage.StaticDraw) where T : struct
        {
            return addArrayBuffer(index, Marshal.SizeOf<T>() / getDataTypeSize(type), data, type, normalized, usage);
        }

        private int getDataTypeSize(DataType type)
        {
            switch (type)
            {
                case DataType.Byte: return 1;
                case DataType.UnsignedByte: return 1;
                case DataType.Short: return 2;
                case DataType.UnsignedShort: return 2;
                case DataType.Int: return 4;
                case DataType.UnsignedInt: return 4;
                case DataType.Float: return 4;
                case DataType.Double: return 8;
                default: return 4;
            }
        }

        public ArrayBuffer addArrayBuffer<T>(uint index, int size, T[] data, bool normalized = false, BufferUsage usage = BufferUsage.StaticDraw) where T : struct
        {
            return addArrayBuffer(index, size, data, ToGlType<T>(), normalized, usage);
        }

        public ArrayBuffer addArrayBuffer<T>(uint index, int size, T[] data, DataType type, bool normalized = false, BufferUsage usage = BufferUsage.StaticDraw) where T : struct
        {
            bind();
            if (!arrayBuffers.ContainsKey(index))
            {
                arrayBuffers.Add(index, new ArrayBuffer());
            }
            ArrayBuffer buffer = arrayBuffers[index];
            buffer.setData(data, usage);
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(short) || typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                glVertexAttribIPointer(index, size, (uint)type, 0, NULL);
            }
            else
            {
                glVertexAttribPointer(index, size, (uint)type, normalized ? (byte)1 : (byte)0, 0, NULL);
            }
            buffer.elementSize = size;

            return buffer;
        }

        public void draw(DrawMode mode, int count, int offset = 0)
        {
            bind();
            if (elementBuffer == null)
            {
                glDrawArrays((uint)mode, offset, count);
            }
            else
            {
                glDrawElements((uint)mode, elementBuffer.length, GL_UNSIGNED_INT, NULL);
            }
        }

        public void draw(DrawMode mode)
        {
            draw(mode, arrayBuffers[0].length / arrayBuffers[0].elementSize);
        }

        public void enableAttributes()
        {
            foreach (uint attrib in arrayBuffers.Keys)
            {
                if (!attribArrays.Contains(attrib))
                {
                    glEnableVertexAttribArray(attrib);
                    attribArrays.Add(attrib);
                }
            }
        }

        public void disableAttributes()
        {
            foreach (uint attrib in arrayBuffers.Keys)
            {
                if (attribArrays.Contains(attrib))
                {
                    glDisableVertexAttribArray(attrib);
                    attribArrays.Remove(attrib);
                }
            }
        }

        public static void enableAttributes(params uint[] attribs)
        {
            foreach (uint attrib in attribs)
            {
                if (!attribArrays.Contains(attrib))
                {
                    glEnableVertexAttribArray(attrib);
                    attribArrays.Add(attrib);
                }
            }
        }

        public static void disableAttributes(params uint[] attribs)
        {
            foreach (uint attrib in attribs)
            {
                if (attribArrays.Contains(attrib))
                {
                    glDisableVertexAttribArray(attrib);
                    attribArrays.Remove(attrib);
                }
            }
        }
    }

    #endregion

    #region Buffer

    public enum BufferUsage : uint
    {
        StaticDraw = GL_STATIC_DRAW,
        DynamicDraw = GL_DYNAMIC_DRAW,
        StreamDraw = GL_STREAM_DRAW
    }

    public enum BufferTarget : uint
    {
        ArrayBuffer = GL_ARRAY_BUFFER,
        ElementArrayBuffer = GL_ELEMENT_ARRAY_BUFFER
    }

    public class ArrayBuffer : Buffer
    {
        internal int elementSize;

        public ArrayBuffer() : base(BufferTarget.ArrayBuffer)
        { }
    }

    public class ElementArrayBuffer : Buffer
    {
        internal DataType type;

        public ElementArrayBuffer() : base(BufferTarget.ElementArrayBuffer)
        { }

        public override void setData<T>(T[] data, BufferUsage usage)
        {
            base.setData<T>(data, usage);
            type = ToGlType<T>();
        }

        public override void setData(float[] data, BufferUsage usage)
        {
            throw new NotImplementedException("Data type FLOAT is not supported for element array buffers");
        }

        public override void setData(double[] data, BufferUsage usage)
        {
            throw new NotImplementedException("Data type DOUBLE is not supported for element array buffers");
        }
    }

    public abstract class Buffer
    {
        private const uint NULL = 0;

        private static Dictionary<BufferTarget, Buffer> bound;

        static Buffer()
        {
            bound = new Dictionary<BufferTarget, Buffer>();
            foreach (BufferTarget target in Enum.GetValues(typeof(BufferTarget)))
            {
                bound.Add(target, null);
            }
        }

        public static DataType ToGlType<T>()
        {
            if (typeof(T) == typeof(byte))
            {
                return DataType.Byte;
            }
            else if (typeof(T) == typeof(short))
            {
                return DataType.UnsignedShort;
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                return DataType.UnsignedInt;
            }
            else if (typeof(T) == typeof(float))
            {
                return DataType.Float;
            }
            else if (typeof(T) == typeof(double))
            {
                return DataType.Double;
            }
            throw new NotImplementedException("Unknown type " + typeof(T).ToString());
        }

        private uint handle;
        private BufferTarget target;

        internal int length;

        internal Buffer(BufferTarget target)
        {
            glGenBuffers(1, ref handle);
            this.target = target;
        }

        ~Buffer()
        {
            if (bound[target] == this)
            {
                unbind();
            }
            glDeleteBuffers(1, ref handle);
        }

        public void bind()
        {
            if (bound[target] != this)
            {
                glBindBuffer((uint)target, handle);
                bound[target] = this;
            }
        }

        public void unbind()
        {
            if (bound[target] != null)
            {
                glBindBuffer((uint)target, NULL);
                bound[target] = null;
            }
        }

        public virtual void setData(byte[] data, BufferUsage usage)
        {
            setData<byte>(data, usage);
        }

        public virtual void setData(short[] data, BufferUsage usage)
        {
            setData<short>(data, usage);
        }

        public virtual void setData(int[] data, BufferUsage usage)
        {
            setData<int>(data, usage);
        }

        public virtual void setData(uint[] data, BufferUsage usage)
        {
            setData<uint>(data, usage);
        }

        public virtual void setData(float[] data, BufferUsage usage)
        {
            setData<float>(data, usage);
        }

        public virtual void setData(double[] data, BufferUsage usage)
        {
            setData<double>(data, usage);
        }

        public virtual void setData<T>(T[] data, BufferUsage usage) where T : struct
        {
            bind();
            GCHandle gc = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr dataPtr = gc.AddrOfPinnedObject();
            glBufferData((uint)target, data.Length * Marshal.SizeOf<T>(), dataPtr, (uint)usage);
            gc.Free();

            length = data.Length;
        }
    }

    #endregion

    #region Shader

    public class Shader
    {
        private static Shader bound;

        static Shader()
        {
            bound = null;
        }

        private uint program;
        private uint vertex;
        private uint fragment;

        private Dictionary<string, int> uniforms;

        public Shader(string vertexSrc, string fragmentSrc)
        {
            program = glCreateProgram();
            vertex = glCreateShader(GL_VERTEX_SHADER);
            fragment = glCreateShader(GL_FRAGMENT_SHADER);

            uniforms = new Dictionary<string, int>();

            vertex = csglShader(vertexSrc, GL_VERTEX_SHADER);
            fragment = csglShader(fragmentSrc, GL_FRAGMENT_SHADER);
            program = csglShaderProgram(vertex, fragment);

            glDeleteShader(vertex);
            glDeleteShader(fragment);
        }

        ~Shader()
        {
            stop();
            glDetachShader(program, vertex);
            glDetachShader(program, fragment);
            glDeleteProgram(program);
        }

        public void start()
        {
            if (bound != this)
            {
                glUseProgram(program);
                bound = this;
            }
        }

        public void stop()
        {
            if (bound != null)
            {
                glUseProgram(0);
                bound = null;
            }
        }

        public int this[string name, int index] { set { glUniform1i(getUniformLocation(name + "[" + index + "]"), value); } }

        private int getUniformLocation(string name)
        {
            if (!uniforms.ContainsKey(name))
            {
                uniforms.Add(name, glGetUniformLocation(program, name));
            }
            return uniforms[name];
        }
    }

    #endregion

    #region Texture

    public enum TextureTarget : uint
    {
        Texture2D = GL_TEXTURE_2D
    }

    public class Texture2D : Texture
    {
        public Texture2D() : base(TextureTarget.Texture2D)
        { }

        public override void setData(int width, int height, int[] pixels, int level = 0)
        {
            bind();
            IntPtr dataPtr = Marshal.AllocHGlobal(pixels.Length * sizeof(int));
            Marshal.Copy(pixels, 0, dataPtr, pixels.Length);
            glTexImage2D((uint)target, level, (int)GL_RGBA8, width, height, 0, GL_BGRA, GL_UNSIGNED_BYTE, dataPtr);
            Marshal.FreeHGlobal(dataPtr);
        }

        public override void setData(Bitmap image)
        {
            int[] pixels = new int[image.Width * image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    pixels[x + y * image.Width] = pixel.ToArgb();
                }
            }
            setData(image.Width, image.Height, pixels, 0);
        }
    }

    public abstract class Texture
    {
        private static Dictionary<TextureTarget, Texture> bound;

        static Texture()
        {
            bound = new Dictionary<TextureTarget, Texture>();
            foreach (TextureTarget target in Enum.GetValues(typeof(TextureTarget)))
            {
                bound.Add(target, null);
            }
        }

        private uint handle;
        protected TextureTarget target;

        public Texture(TextureTarget target)
        {
            glGenTextures(1, ref handle);
            this.target = target;

            glBindTexture((uint)target, handle);
            glTexParameteri((uint)target, GL_TEXTURE_MIN_FILTER, (int)GL_NEAREST);
            glTexParameteri((uint)target, GL_TEXTURE_MAG_FILTER, (int)GL_NEAREST);
            glTexParameteri((uint)target, GL_TEXTURE_WRAP_S, (int)GL_REPEAT);
            glTexParameteri((uint)target, GL_TEXTURE_WRAP_T, (int)GL_REPEAT);
            glBindTexture((uint)target, 0);
        }

        ~Texture()
        {
            unbind();
            glDeleteTextures(1, new uint[] { handle });
        }

        public void bind()
        {
            if (bound[target] != this)
            {
                glBindTexture((uint)target, handle);
                bound[target] = this;
            }
        }

        public void unbind()
        {
            if (bound[target] != null)
            {
                glBindTexture((uint)target, 0);
                bound[target] = null;
            }
        }

        public abstract void setData(int width, int height, int[] pixels, int level = 0);

        public abstract void setData(Bitmap image);
    }

    #endregion

}