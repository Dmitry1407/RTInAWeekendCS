using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.plane
{
    public class YZPlane : Plane
    {
        private Single dy, dz;
        public Single Y0 { get; set; }
        public Single Y1 { get; set; }
        public Single Z0 { get; set; }
        public Single Z1 { get; set; }

        public YZPlane(Single y0, Single y1, Single z0, Single z1, Single x, Material material)
        {
            PType = PlaneType.YZ;
            Normal = new Vector3F(1F, 0F, 0F);
            Material = material;
            Y0 = y0; Y1 = y1; dy = y1 - y0;
            Z0 = z0; Z1 = z1; dz = z1 - z0;
            K = x;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            Single t = (K - ray.Origin.X) / ray.Direction.X;
            if (t < tMin || t > tMax)
            {
                return null;
            }

            Single y = ray.Origin.Y + t * ray.Direction.Y;
            Single z = ray.Origin.Z + t * ray.Direction.Z;
            if (y < Y0 || y > Y1 || z < Z0 || z > Z1)
            {
                return null;
            }

            Record.T = t;
            Record.U = (y - Y0) / dy;
            Record.V = (z - Z0) / dz;
            Record.Position = ray.PointAtParameter(t);
            Record.Normal.CopyFrom(Normal);
            if (FlipNormales) Record.Normal.Mul(-1F);
            return Record;
        }
    }
}
