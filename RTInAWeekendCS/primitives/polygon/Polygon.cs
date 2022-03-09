using System;
using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;

namespace RTInAWeekendCS.primitives.polygon
{
    public class Polygon : Primitive
    {
        public Vector3F[] Vertices { get; protected set; }
        public int[] Indexes { get; protected set; }

        public Polygon(Vector3F v1, Vector3F v2, Vector3F v3, Material material)
        {
            Vertices = new[] { v1, v2, v3 };
            Indexes = new[] { 0, 1, 2 };

            this.Material = material;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax) { return null; }
    }
}
