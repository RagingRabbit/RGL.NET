using System;
using System.Collections.Generic;

using static CSGL.CSGL;
using static CSGL.OpenGL;

namespace RGL
{

    #region Vector2

    public struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public int glSize { get { return 2; } }
        public DataType glType { get { return DataType.Float; } }

        public Vector2 xy { get { return new Vector2(x, y); } }
        public Vector2 yx { get { return new Vector2(y, x); } }
    }

    #endregion

    #region Vector3

    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector2 xy, float z)
        {
            this.x = xy.x;
            this.y = xy.y;
            this.z = z;
        }

        public Vector3(float x, Vector2 yz)
        {
            this.x = x;
            this.y = yz.x;
            this.z = yz.y;
        }

        public int glSize { get { return 3; } }
        public DataType glType { get { return DataType.Float; } }

        public Vector3 xyz { get { return new Vector3(x, y, z); } }
        public Vector3 xzy { get { return new Vector3(x, z, y); } }
        public Vector3 yxz { get { return new Vector3(y, x, z); } }
        public Vector3 yzx { get { return new Vector3(y, z, x); } }
        public Vector3 zxy { get { return new Vector3(z, x, y); } }
        public Vector3 zyx { get { return new Vector3(z, y, x); } }

        public Vector2 xy { get { return new Vector2(x, y); } }
        public Vector2 yz { get { return new Vector2(y, z); } }
        public Vector2 xz { get { return new Vector2(x, z); } }

        public override string ToString() { return x + "," + y + "," + z; }
    }

    #endregion

    #region Vector4

    public struct Vector4
    {
        public float x, y, z, w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector3 xyz, float w)
        {
            this.x = xyz.x;
            this.y = xyz.y;
            this.z = xyz.z;
            this.w = w;
        }

        public Vector4(float x, Vector3 yzw)
        {
            this.x = x;
            this.y = yzw.x;
            this.z = yzw.y;
            this.w = yzw.z;
        }

        public Vector4(Vector2 xy, float z, float w)
        {
            this.x = xy.x;
            this.y = xy.y;
            this.z = z;
            this.w = w;
        }

        public Vector4(float x, Vector2 yz, float w)
        {
            this.x = x;
            this.y = yz.x;
            this.z = yz.y;
            this.w = w;
        }

        public Vector4(float x, float y, Vector2 zw)
        {
            this.x = x;
            this.y = y;
            this.z = zw.x;
            this.w = zw.y;
        }

        public Vector4(Vector2 xy, Vector2 zw)
        {
            this.x = xy.x;
            this.y = xy.y;
            this.z = zw.x;
            this.w = zw.y;
        }

        public int glSize { get { return 4; } }
        public DataType glType { get { return DataType.Float; } }

        public Vector4 xyzw { get { return new Vector4(x, y, z, w); } }
        public Vector4 xywz { get { return new Vector4(x, y, w, z); } }
        public Vector4 xzyw { get { return new Vector4(x, z, y, w); } }
        public Vector4 xzwy { get { return new Vector4(x, z, w, y); } }
        public Vector4 yxzw { get { return new Vector4(y, x, z, w); } }
        public Vector4 yxwz { get { return new Vector4(y, x, w, z); } }
        public Vector4 yzxw { get { return new Vector4(y, z, x, w); } }
        public Vector4 yzwx { get { return new Vector4(y, z, w, x); } }
        public Vector4 zxyw { get { return new Vector4(z, x, y, w); } }
        public Vector4 zxwy { get { return new Vector4(z, x, w, y); } }
        public Vector4 zyxw { get { return new Vector4(z, y, x, w); } }
        public Vector4 zywx { get { return new Vector4(z, y, w, x); } }

        public Vector3 xyz { get { return new Vector3(x, y, z); } }
        public Vector3 xzy { get { return new Vector3(x, z, y); } }
        public Vector3 yxz { get { return new Vector3(y, x, z); } }
        public Vector3 yzx { get { return new Vector3(y, z, x); } }
        public Vector3 zxy { get { return new Vector3(z, x, y); } }
        public Vector3 zyx { get { return new Vector3(z, y, x); } }
        public Vector3 wxy { get { return new Vector3(w, x, y); } }
        public Vector3 wxz { get { return new Vector3(w, x, z); } }
        public Vector3 wyx { get { return new Vector3(w, y, x); } }
        public Vector3 wyz { get { return new Vector3(w, y, z); } }
        public Vector3 wzx { get { return new Vector3(w, z, x); } }
        public Vector3 wzy { get { return new Vector3(w, z, y); } }
        public Vector3 xwy { get { return new Vector3(x, w, y); } }
        public Vector3 xwz { get { return new Vector3(x, w, z); } }
        public Vector3 ywx { get { return new Vector3(y, w, x); } }
        public Vector3 ywz { get { return new Vector3(y, w, z); } }
        public Vector3 xyw { get { return new Vector3(x, y, w); } }
        public Vector3 xzw { get { return new Vector3(x, z, w); } }
        public Vector3 yxw { get { return new Vector3(y, x, w); } }
        public Vector3 yzw { get { return new Vector3(y, z, w); } }
        public Vector3 zxw { get { return new Vector3(z, x, w); } }
        public Vector3 zyw { get { return new Vector3(z, y, w); } }

        public Vector2 xy { get { return new Vector2(x, y); } }
        public Vector2 xz { get { return new Vector2(x, z); } }
        public Vector2 xw { get { return new Vector2(x, w); } }
        public Vector2 yx { get { return new Vector2(y, x); } }
        public Vector2 yz { get { return new Vector2(y, z); } }
        public Vector2 yw { get { return new Vector2(y, w); } }
        public Vector2 zx { get { return new Vector2(z, x); } }
        public Vector2 zy { get { return new Vector2(z, y); } }
        public Vector2 zw { get { return new Vector2(z, w); } }
        public Vector2 wx { get { return new Vector2(w, x); } }
        public Vector2 wy { get { return new Vector2(w, y); } }
        public Vector2 wz { get { return new Vector2(w, z); } }
    }

    #endregion

    #region Matrix4

    public struct Matrix4
    {
        public static Matrix4 Identity { get { return new Matrix4(new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }); } }

        private static readonly float[] buffer3 = new float[3];
        private static readonly float[] buffer4 = new float[4];

        private float[] elements;

        public Matrix4(float[] elements)
        {
            this.elements = new float[16];
            Array.Copy(elements, this.elements, Math.Min(elements.Length, this.elements.Length));
        }

        public void setIdentity()
        {
            elements[0] = 1.0f;
            elements[5] = 1.0f;
            elements[10] = 1.0f;
            elements[15] = 1.0f;
        }

        public float this[uint x, uint y]
        {
            get { return elements[x * 4 + y]; }
            set { elements[x * 4 + y] = value; }
        }

        public float[] array { get { return elements; } }

        public Vector4 column0 { get { return new Vector4(elements[0], elements[1], elements[2], elements[3]); } }
        public Vector4 column1 { get { return new Vector4(elements[4], elements[5], elements[6], elements[7]); } }
        public Vector4 column2 { get { return new Vector4(elements[8], elements[9], elements[10], elements[11]); } }
        public Vector4 column3 { get { return new Vector4(elements[12], elements[13], elements[14], elements[15]); } }

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
        {
            return new Matrix4(csglMatrixMul(m1.elements, m2.elements, 4));
        }

        public static Vector4 operator *(Matrix4 m, Vector4 v)
        {
            return (Translate(v) * m).column3;
        }

        public static Matrix4 Ortho(float left, float bottom, float right, float top, float near = 1.0f, float far = -1.0f)
        {
            return new Matrix4(csglMatrixOrtho(left, right, bottom, top, near, far));
        }

        public static Matrix4 Perspective(float fovy, float aspect, float near, float far)
        {
            float rad = fovy / 180.0f * MathF.PI;
            float y = 1.0f / MathF.Tan(0.5f * rad) * aspect;
            float x = y / aspect;
            float l = far - near;

            Matrix4 m = new Matrix4();
            m[0, 0] = x;
            m[1, 1] = y;
            m[2, 2] = (far + near) / -l;
            m[2, 3] = -1.0f;
            m[3, 2] = -2.0f * near * far / l;
            m[3, 3] = 0.0f;

            return m;
        }

        public static Matrix4 Translate(Vector3 v)
        {
            Matrix4 m = new Matrix4();
            m[3, 0] = v.x;
            m[3, 1] = v.y;
            m[3, 2] = v.z;
            return m;
        }

        public static Matrix4 Translate(Vector4 v)
        {
            Matrix4 m = new Matrix4();
            m[3, 0] = v.x;
            m[3, 1] = v.y;
            m[3, 2] = v.z;
            m[3, 3] = v.w;
            return m;
        }

        public static Matrix4 Scale(Vector3 v)
        {
            Matrix4 m = new Matrix4();
            m[0, 0] = v.x;
            m[1, 1] = v.y;
            m[2, 2] = v.z;
            return m;
        }

        public static Matrix4 Scale(Vector4 v)
        {
            Matrix4 m = new Matrix4();
            m[0, 0] = v.x;
            m[1, 1] = v.y;
            m[2, 2] = v.z;
            m[3, 3] = v.w;
            return m;
        }
    }

    #endregion
}