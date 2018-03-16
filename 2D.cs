using System;
using System.Collections.Generic;

namespace RGL
{
    public class SpriteBatch
    {
        private const int INDEX_POSITION = 0;
        private const int INDEX_COLOR = 1;
        private const int INDEX_UV = 2;
        private const int INDEX_TEXTURE = 3;

        private const int SIZE_POSITION = 3 * sizeof(float);
        private const int SIZE_COLOR = 4 * sizeof(byte);
        private const int SIZE_UV = 2 * sizeof(float);
        private const int SIZE_TEXTURE = 1 * sizeof(byte);

        private const int SIZE_VERTEX = SIZE_POSITION + SIZE_COLOR + SIZE_UV + SIZE_TEXTURE;
        private const int SIZE_SPRITE = 4 * SIZE_VERTEX;

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
        }

        public void draw(float x, float y, float z, float width, float height, Texture texture)
        {
            positions.Add(new Vector3(x, y, z));
            colors.Add(new Vector4(1, 1, 1, 1));
            uv.Add(new Vector2(0.0f, 1.0f));
            textures.Add(0.0f);

            positions.Add(new Vector3(x + width, y, z));
            colors.Add(new Vector4(1, 1, 1, 1));
            uv.Add(new Vector2(0.0f, 1.0f));
            textures.Add(0.0f);

            positions.Add(new Vector3(x + width, y + height, z));
            colors.Add(new Vector4(1, 1, 1, 1));
            uv.Add(new Vector2(0.0f, 1.0f));
            textures.Add(0.0f);

            positions.Add(new Vector3(x, y + height, z));
            colors.Add(new Vector4(1, 1, 1, 1));
            uv.Add(new Vector2(0.0f, 1.0f));
            textures.Add(0.0f);

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

            vao.bind();

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

            vao.enableAttributes();

            vao.draw(DrawMode.Triangles, spriteCount);

            vao.unbind();

            shader.stop();

            positions.Clear();
            colors.Clear();
            uv.Clear();
            textures.Clear();
            indices.Clear();

            indexPtr = 0;
            spriteCount = 0;
        }
    }
}