using System;

namespace RTInAWeekendCS.utils
{
    public class Vector3F
    {
        private Single x;
        private Single y;
        private Single z;

        public Vector3F() : this(0F, 0F, 0F) { }

        public Vector3F(Single x, Single y, Single z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Single X { get { return x; } set { x = value; } }
        public Single Y { get { return y; } set { y = value; } }
        public Single Z { get { return z; } set { z = value; } }

        public Single Length() { return (Single)Math.Sqrt(x * x + y * y + z * z); }

        public Single SqrtLength() { return x * x + y * y + z * z; }

        public static Vector3F operator +(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3F operator -(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3F operator *(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3F operator /(Vector3F v1, Vector3F v2)
        {
            return new Vector3F(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static Vector3F operator *(Single t, Vector3F v)
        {
            return new Vector3F(t * v.x, t * v.y, t * v.z);
        }

        public static Vector3F operator /(Vector3F v, Single t)
        {
            return new Vector3F(v.x / t, v.y / t, v.z / t);
        }

        public static void Add(Vector3F dest, Vector3F vec1, Vector3F vec2)
        {
            dest.x = vec1.x + vec2.x;
            dest.y = vec1.y + vec2.y;
            dest.z = vec1.z + vec2.z;
        }

        public static void Add(Vector3F dest, Vector3F vec1, Single t)
        {
            dest.x = vec1.x + t;
            dest.y = vec1.y + t;
            dest.z = vec1.z + t;
        }

        public static void Sub(Vector3F dest, Vector3F vec1, Vector3F vec2)
        {
            dest.x = vec1.x - vec2.x;
            dest.y = vec1.y - vec2.y;
            dest.z = vec1.z - vec2.z;
        }

        public static void Sub(Vector3F dest, Vector3F vec1, Single t)
        {
            dest.x = vec1.x - t;
            dest.y = vec1.y - t;
            dest.z = vec1.z - t;
        }

        public static void Mul(Vector3F dest, Vector3F vec1, Vector3F vec2)
        {
            dest.x = vec1.x * vec2.x;
            dest.y = vec1.y * vec2.y;
            dest.z = vec1.z * vec2.z;
        }

        public static void Mul(Vector3F dest, Vector3F vec1, Single t)
        {
            dest.x = vec1.x * t;
            dest.y = vec1.y * t;
            dest.z = vec1.z * t;
        }

        public static void Div(Vector3F dest, Vector3F vec1, Vector3F vec2)
        {
            dest.x = vec1.x / vec2.x;
            dest.y = vec1.y / vec2.y;
            dest.z = vec1.z / vec2.z;
        }

        public static void Div(Vector3F dest, Vector3F vec1, Single t)
        {
            dest.x = vec1.x / t;
            dest.y = vec1.y / t;
            dest.z = vec1.z / t;
        }

        public void Add(Vector3F other) { this.x += other.x; this.y += other.y; this.z += other.z; }

        public void Add(Single t) { this.x += t; this.y += t; this.z += t; }

        public void Sub(Vector3F other) { this.x -= other.x; this.y -= other.y; this.z -= other.z; }

        public void Sub(Single t) { this.x -= t; this.y -= t; this.z -= t; }

        public void Mul(Vector3F other) { this.x *= other.x; this.y *= other.y; this.z *= other.z; }

        public void Mul(Single t) { this.x *= t; this.y *= t; this.z *= t; }

        public void Div(Vector3F other) { this.x /= other.x; this.y /= other.y; this.z /= other.z; }

        public void Div(Single t) { this.x /= t; this.y /= t; this.z /= t; }

        public void Reset() { this.x = 0F; this.y = 0F; this.z = 0F; }

        public void CopyFrom(Vector3F other) { this.x = other.x; this.y = other.y; this.z = other.z; }

        public void SetXYZ(Single x, Single y, Single z) { this.x = x; this.y = y; this.z = z; }

        public void Normalize() { this.Div(this.Length()); }

        public static Vector3F UnitVector(Vector3F vec) { return vec / vec.Length(); }

        public static Single Dot(Vector3F v1, Vector3F v2) { return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z; }

        public static Vector3F Cross(Vector3F v1, Vector3F v2)
        {
            return new Vector3F((v1.y * v2.z - v1.z * v2.y),
                          (-(v1.x * v2.z - v1.z * v2.x)),
                            (v1.x * v2.y - v1.y * v2.x));
        }

        public static void Cross(Vector3F dest, Vector3F v1, Vector3F v2)
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
