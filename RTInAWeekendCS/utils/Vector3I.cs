using System;

namespace RTInAWeekendCS.utils
{
    public class Vector3I
    {
        private Int32 x;
        private Int32 y;
        private Int32 z;

        public Vector3I() : this(0, 0, 0) { }

        public Vector3I(Int32 x, Int32 y, Int32 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Int32 X { get { return x; } set { x = value; } }
        public Int32 Y { get { return y; } set { y = value; } }
        public Int32 Z { get { return z; } set { z = value; } }

        public Int32 Length() { return (Int32)Math.Sqrt(x * x + y * y + z * z); }

        public Int32 SqrtLength() { return x * x + y * y + z * z; }

        public static Vector3I operator +(Vector3I v1, Vector3I v2)
        {
            return new Vector3I(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3I operator -(Vector3I v1, Vector3I v2)
        {
            return new Vector3I(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3I operator *(Vector3I v1, Vector3I v2)
        {
            return new Vector3I(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3I operator /(Vector3I v1, Vector3I v2)
        {
            return new Vector3I(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vector3I operator *(Int32 t, Vector3I v)
        {
            return new Vector3I(t * v.x, t * v.y, t * v.z);
        }

        public static Vector3I operator /(Vector3I v, Int32 t)
        {
            return new Vector3I(v.x / t, v.y / t, v.z / t);
        }

        public static void Add(Vector3I dest, Vector3I vec1, Vector3I vec2)
        {
            dest.x = vec1.x + vec2.x;
            dest.y = vec1.y + vec2.y;
            dest.z = vec1.z + vec2.z;
        }

        public static void Add(Vector3I dest, Vector3I vec1, Int32 t)
        {
            dest.x = vec1.x + t;
            dest.y = vec1.y + t;
            dest.z = vec1.z + t;
        }

        public static void Sub(Vector3I dest, Vector3I vec1, Vector3I vec2)
        {
            dest.x = vec1.x - vec2.x;
            dest.y = vec1.y - vec2.y;
            dest.z = vec1.z - vec2.z;
        }

        public static void Sub(Vector3I dest, Vector3I vec1, Int32 t)
        {
            dest.x = vec1.x - t;
            dest.y = vec1.y - t;
            dest.z = vec1.z - t;
        }

        public static void Mul(Vector3I dest, Vector3I vec1, Vector3I vec2)
        {
            dest.x = vec1.x * vec2.x;
            dest.y = vec1.y * vec2.y;
            dest.z = vec1.z * vec2.z;
        }

        public static void Mul(Vector3I dest, Vector3I vec1, Single t)
        {
            dest.x = (int)(vec1.x * t);
            dest.y = (int)(vec1.y * t);
            dest.z = (int)(vec1.z * t);
        }

        public static void Div(Vector3I dest, Vector3I vec1, Vector3I vec2)
        {
            dest.x = vec1.x / vec2.x;
            dest.y = vec1.y / vec2.y;
            dest.z = vec1.z / vec2.z;
        }

        public static void Div(Vector3I dest, Vector3I vec1, Int32 t)
        {
            dest.x = vec1.x / t;
            dest.y = vec1.y / t;
            dest.z = vec1.z / t;
        }

        public void Add(Vector3I other) { this.x += other.x; this.y += other.y; this.z += other.z; }

        public void Add(Int32 t) { this.x += t; this.y += t; this.z += t; }

        public void Sub(Vector3I other) { this.x -= other.x; this.y -= other.y; this.z -= other.z; }

        public void Sub(Int32 t) { this.x -= t; this.y -= t; this.z -= t; }

        public void Mul(Vector3I other) { this.x *= other.x; this.y *= other.y; this.z *= other.z; }

        public void Mul(Single t) { this.x = (Int32)(x * t + 0.5F); this.y = (Int32)(y * t + 0.5F); this.z = (Int32)(z * t + 0.5F); }

        public void Div(Vector3I other) { this.x /= other.x; this.y /= other.y; this.z /= other.z; }

        public void Div(Int32 t) { this.x /= t; this.y /= t; this.z /= t; }

        public void Reset() { this.x = 0; this.y = 0; this.z = 0; }

        public void CopyFrom(Vector3I other) { this.x = other.x; this.y = other.y; this.z = other.z; }

        public void SetXYZ(Int32 x, Int32 y, Int32 z) { this.x = x; this.y = y; this.z = z; }

        public void Normalize() { this.Div(this.Length()); }

        public static Vector3I UnitVector(Vector3I vec) { return vec / vec.Length(); }

        public static Int32 Dot(Vector3I v1, Vector3I v2) { return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z; }

        public static Vector3I Cross(Vector3I v1, Vector3I v2)
        {
            return new Vector3I((v1.y * v2.z - v1.z * v2.y),
                          (-(v1.x * v2.z - v1.z * v2.x)),
                            (v1.x * v2.y - v1.y * v2.x));
        }

        public static void Cross(Vector3I dest, Vector3I v1, Vector3I v2)
        {
            dest.x = v1.y * v2.z - v1.z * v2.y;
            dest.y = -(v1.x * v2.z - v1.z * v2.x);
            dest.z = v1.x * v2.y - v1.y * v2.x;
        }

        public override String ToString()
        {
            return String.Format("Vector [X:{0:F2}] [Y:{1:F2}] [Z:{2:F2}]", x, y, z);
        }
    }
}
