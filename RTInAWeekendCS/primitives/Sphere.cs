using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{
    public class Sphere : Primitive
    {
        private Vector3F vtmp = new Vector3F();
        private Single rp2;

        public Sphere() { }

        public Sphere(Vector3F center, Single radius, Material material)
        {
            this.Center = center;
            this.Radius = radius;
            this.rp2 = radius * radius;
            this.Material = material;
            Record = new HitRecord();
            Record.Material = Material;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            Vector3F.Sub(vtmp, ray.Origin, Center);
            Single a = Vector3F.Dot(ray.Direction, ray.Direction);
            Single b = Vector3F.Dot(vtmp, ray.Direction);
            Single c = Vector3F.Dot(vtmp, vtmp) - rp2;
            Single discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                Single temp = (Single)(-b - Math.Sqrt(discriminant)) / a;
                if (temp > tMin && temp < tMax)
                {
                    Record.T = temp;
                    Record.Position.CopyFrom(ray.PointAtParameter(temp));
                    ////record.Normal = (record.P - center) / radius;
                    Vector3F.Sub(vtmp, Record.Position, Center);
                    vtmp.Div(Radius);
                    Record.Normal.CopyFrom(vtmp);
                    return Record;
                }
                temp = (Single)(-b + Math.Sqrt(discriminant)) / a;
                if (temp > tMin && temp < tMax)
                {
                    Record.T = temp;
                    Record.Position.CopyFrom(ray.PointAtParameter(temp));
                    ////record.Normal = (record.P - center) / radius;
                    Vector3F.Sub(vtmp, Record.Position, Center);
                    vtmp.Div(Radius);
                    Record.Normal.CopyFrom(vtmp);
                    return Record;
                }
            }
            return null;
        }
    }
}
