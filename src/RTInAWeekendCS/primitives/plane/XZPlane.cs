using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.plane
{
    public class XZPlane : Plane
    {
        private Single dx, dz;
        public Single X0 { get; set; }
        public Single X1 { get; set; }
        public Single Z0 { get; set; }
        public Single Z1 { get; set; }

        public XZPlane(Single x0, Single x1, Single z0, Single z1, Single y, Material material)
        {
            PType = PlaneType.XZ;
            Normal = new Vector3F(0F, 1F, 0F);
            Material = material;
            X0 = x0; X1 = x1; dx = x1 - x0;
            Z0 = z0; Z1 = z1; dz = z1 - z0;
            K = y;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            Single t = (K - ray.Origin.Y) / ray.Direction.Y;
            if (t < tMin || t > tMax)
            {
                return null;
            }

            Single x = ray.Origin.X + t * ray.Direction.X;
            Single z = ray.Origin.Z + t * ray.Direction.Z;
            if (x < X0 || x > X1 || z < Z0 || z > Z1)
            {
                return null;
            }

            Record.T = t;
            Record.U = (x - X0) / dx;
            Record.V = (z - Z0) / dz;
            Record.Position = ray.PointAtParameter(t);
            Record.Normal.CopyFrom(Normal);
            if (FlipNormales) Record.Normal.Mul(-1F);
            return Record;
        }
    }
}
