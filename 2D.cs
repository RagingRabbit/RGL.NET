using System;
using System.Drawing;
using System.Collections.Generic;

namespace RGL
{
    #region SpriteRenderer

    public class SpriteBatch
    {
        private VertexArray vao;
        private ArrayBuffer vbo0;
        private ArrayBuffer vbo1;
        private ArrayBuffer vbo2;
        private ArrayBuffer vbo3;
        private ElementArrayBuffer ibo;
        private Shader shader;

        private List<Texture2D> textureCache;
        private uint indexPtr;
        private int spriteCount;

        private List<Vector3> positions;
        private List<Vector4> colors;
        private List<Vector2> uv;
        private List<float> textures;
        private List<uint> indices;

        private Matrix4 mvp;

        public SpriteBatch(Shader shader)
        {
            textureCache = new List<Texture2D>();
            indexPtr = 0;
            spriteCount = 0;

            positions = new List<Vector3>();
            colors = new List<Vector4>();
            uv = new List<Vector2>();
            textures = new List<float>();
            indices = new List<uint>();

            vao = new VertexArray();
            vbo0 = vao.addArrayBuffer<Vector3>(0, new Vector3[0], DataType.Float, false, BufferUsage.StreamDraw);
            vbo1 = vao.addArrayBuffer<Vector4>(1, new Vector4[0], DataType.Float, false, BufferUsage.StreamDraw);
            vbo2 = vao.addArrayBuffer<Vector2>(2, new Vector2[0], DataType.Float, false, BufferUsage.StreamDraw);
            vbo3 = vao.addArrayBuffer<float>(3, new float[0], DataType.Float, false, BufferUsage.StreamDraw);
            ibo = vao.addElementBuffer<uint>(new uint[0], BufferUsage.StreamDraw);

            this.shader = shader;
            shader.start();
            for (int i = 0; i < 32; i++)
            {
                shader["un_Textures", i] = i;
            }
            shader.stop();

            mvp = Matrix4.Identity;
        }

        public void draw(float x, float y, float z, float width, float height, Texture2D texture, Color tint, float u0 = 0.0f, float v0 = 0.0f, float u1 = 1.0f, float v1 = 1.0f)
        {
            float textureID = 0.0f;

            if (textureCache.Contains(texture))
            {
                textureID = textureCache.IndexOf(texture);
            }
            else
            {
                textureID = textureCache.Count;
                textureCache.Add(texture);
            }

            positions.Add(new Vector3(x, y, z));
            colors.Add(new Vector4(tint.R / 255.0f, tint.G / 255.0f, tint.B / 255.0f, tint.A / 255.0f));
            uv.Add(new Vector2(u0, v1));
            textures.Add(textureID);

            positions.Add(new Vector3(x + width, y, z));
            colors.Add(new Vector4(tint.R / 255.0f, tint.G / 255.0f, tint.B / 255.0f, tint.A / 255.0f));
            uv.Add(new Vector2(u1, v1));
            textures.Add(textureID);

            positions.Add(new Vector3(x + width, y + height, z));
            colors.Add(new Vector4(tint.R / 255.0f, tint.G / 255.0f, tint.B / 255.0f, tint.A / 255.0f));
            uv.Add(new Vector2(u1, v0));
            textures.Add(textureID);

            positions.Add(new Vector3(x, y + height, z));
            colors.Add(new Vector4(tint.R / 255.0f, tint.G / 255.0f, tint.B / 255.0f, tint.A / 255.0f));
            uv.Add(new Vector2(u0, v0));
            textures.Add(textureID);

            indices.Add(indexPtr);
            indices.Add(indexPtr + 1);
            indices.Add(indexPtr + 2);
            indices.Add(indexPtr + 2);
            indices.Add(indexPtr + 3);
            indices.Add(indexPtr + 0);

            indexPtr += 4;
            spriteCount++;
        }

        public void show()
        {
            shader.start();

            shader["un_Matrix", false] = mvp;

            for (int i = 0; i < textureCache.Count; i++)
            {
                textureCache[i].bind((uint)i);
            }

            vao.bind();
            vao.enableAttributes();

            vbo0.bind();
            vbo0.setData<Vector3>(positions.ToArray(), BufferUsage.StreamDraw);
            vbo1.bind();
            vbo1.setData<Vector4>(colors.ToArray(), BufferUsage.StreamDraw);
            vbo2.bind();
            vbo2.setData<Vector2>(uv.ToArray(), BufferUsage.StreamDraw);
            vbo2.bind();
            vbo3.setData<float>(textures.ToArray(), BufferUsage.StreamDraw);
            ibo.bind();
            ibo.setData<uint>(indices.ToArray(), BufferUsage.StreamDraw);

            vao.draw(DrawMode.Triangles, spriteCount);

            vao.unbind();

            shader.stop();

            positions.Clear();
            colors.Clear();
            uv.Clear();
            textures.Clear();
            indices.Clear();

            textureCache.Clear();

            indexPtr = 0;
            spriteCount = 0;
        }

        public Matrix4 matrix { set { mvp = value; } }
    }

    #endregion
}