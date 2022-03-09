using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.plane
{
    public class XYPlane : Plane
    {
        private Single dx, dy;
        public Single X0 { get; set; }
        public Single X1 { get; set; }
        public Single Y0 { get; set; }
        public Single Y1 { get; set; }

        public XYPlane(Single x0, Single x1, Single y0, Single y1, Single z, Material material)
        {
            PType = PlaneType.XY;
            Normal = new Vector3F(0F, 0F, 1F);
            Material = material;
            X0 = x0; X1 = x1; dx = x1 - x0;
            Y0 = y0; Y1 = y1; dy = y1 - y0;
            K = z;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            Single t = (K - ray.Origin.Z) / ray.Direction.Z;
            if (t < tMin || t > tMax)
            {
                return null;
            }

            Single x = ray.Origin.X + t * ray.Direction.X;
            Single y = ray.Origin.Y + t * ray.Direction.Y;
            if (x < X0 || x > X1 || y < Y0 || y > Y1)
            {
                return null;
            }

            Record.T = t;
            Record.U = (x - X0) / dx;
            Record.V = (y - Y0) / dy;
            Record.Position = ray.PointAtParameter(t);
            Record.Normal.CopyFrom(Normal);
            if (FlipNormales) Record.Normal.Mul(-1F);
            return Record;
        }
    }
}
