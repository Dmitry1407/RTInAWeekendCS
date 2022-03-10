using System;
using RTInAWeekendCS.utils;
using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.hit;

namespace RTInAWeekendCS.primitives.polygon
{
    public class Triangle : Primitive
    {
        public Vector3F[] Vertices { get; protected set; }
        public Int32[] Indexes { get; protected set; }
        public Vector3F Edge01 { get; protected set; }
        public Vector3F Edge02 { get; protected set; }
        public Vector3F Edge12 { get; protected set; }
        public Vector3F Cross { get; protected set; }

        public Triangle(Vector3F v1, Vector3F v2, Vector3F v3, Material material) : this(v1, v2, v3, 0, 1, 2, material) { }

        public Triangle(Vector3F v1, Vector3F v2, Vector3F v3, Int32 index1, Int32 index2, Int32 index3, Material material)
        {
            Vertices = new[] { v1, v2, v3 };
            Indexes = new[] { index1, index2, index3 };

            Edge01 = new Vector3F();
            Vector3F.Sub(Edge01, v2, v1);
            Edge02 = new Vector3F();
            Vector3F.Sub(Edge02, v3, v1);
            Edge12 = new Vector3F();
            Vector3F.Sub(Edge12, v3, v2);
            Cross = new Vector3F();
            Vector3F.Cross(Cross, Edge01, Edge02);
            Cross.Normalize();
            ////Vector.UnitVector(Cross);
            ////Cross.Mul(-1F);

            this.Material = material;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax) { return null; }
    }
}
