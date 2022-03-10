using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{

    public class Cylinder : Primitive
    {
        private Single y0;
        private Single y1;
        private Single radius;
        private Single inv_radius;
        private Vector3F vtmp = new Vector3F();
        private Single kEpsilon = 0.0001F;

        public Cylinder(Single bottom, Single top, Single r, Material material)
        {
            this.y0 = bottom;
            this.y1 = top;
            this.radius = r;
            Material = material;
            Record = new HitRecord();
            Record.Material = Material;

        }

        ////public override HitRecord Hit(Ray ray, Single tMin, Single tMax) { return null; }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {

            Single t;
            Single ox = ray.Origin.X; Single oy = ray.Origin.Y; Single oz = ray.Origin.Z;
            Single dx = ray.Direction.X; Single dy = ray.Direction.Y; Single dz = ray.Direction.Z;

            Single a = dx * dx + dz * dz;
            Single b = 2F * (ox * dx + oz * dz);
            Single c = ox * ox + oz * oz - radius * radius;
            Single disc = b * b - 4F * a * c;

            if (disc < 0F)
                return (null);

            else
            {
                Single e = (Single)Math.Sqrt(disc);
                Single denom = 2F * a;
                t = (-b - e) / denom; // smaller root

                if (t > kEpsilon)
                {
                    Single yhit = oy + t * dy;
                    if (yhit > y0 && yhit < y1)
                    {
                        tMin = t;

                        ////sr.local_hit_point = ray.o + tMin * ray.d;
                        Vector3F.Mul(vtmp, ray.Direction, t);
                        vtmp.Add(ray.Origin);
                        Record.Position.CopyFrom(vtmp);

                        Record.Normal = new Vector3F((ox + t * dx) * inv_radius, 0F, (oz + t * dz) * inv_radius);
                        // test for hitting from inside
                        ////if (-ray.Direction * Record.Normal < 0F)
                        ////    Record.Normal.Mul(-1F);

                        //Vector.Sub(vtmp, Record.Position, Center);
                        //vtmp.Div(Radius);
                        //Record.Normal.CopyFrom(vtmp);

                        return (Record);
                    }
                }

                t = (-b + e) / denom; // larger root
                if (t > kEpsilon)
                {
                    Single yhit = oy + t * dy;
                    if (yhit > y0 && yhit < y1)
                    {
                        tMin = t;

                        ////sr.local_hit_point = ray.o + tmin * ray.d;
                        Vector3F.Mul(vtmp, ray.Direction, t);
                        vtmp.Add(ray.Origin);
                        Record.Position.CopyFrom(vtmp);

                        Record.Normal = new Vector3F((ox + t * dx) * inv_radius, 0F, (oz + t * dz) * inv_radius);
                        // test for hitting inside surface
                        ////if (-ray.Direction * Record.Normal < 0F)
                        ////    Record.Normal.Mul(-1F);

                        //Vector.Sub(vtmp, Record.Position, Center);
                        //vtmp.Div(Radius);
                        //Record.Normal.CopyFrom(vtmp);

                        return (Record);
                    }
                }
            }

            return (null);
        }
    }
}
