using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material
{
    public class Metal : Material
    {
        private FColor albedo;
        private Single fuzz;
        private Vector3F target = new Vector3F();
        private Vector3F unitVector = new Vector3F();

        public Metal(FColor albedo, Single f)
        {
            this.albedo = albedo;
            fuzz = f < 1F ? f : 1F;
        }

        public override bool Scatter(Ray rayIN, HitRecord record, FColor attenuation, Ray scattered)
        {
            ////attenuation = albedo;
            attenuation.CopyFrom(albedo);
            ////scattered = new Ray(record.P, reflected + fuzz * random_in_unit_sphere());
            Vector3F.Div(unitVector, rayIN.Direction, rayIN.Direction.Length());
            Reflect(unitVector, record.Normal);
            Vector3F.Mul(target, RandomInUnitSphere(), fuzz);
            target.Add(reflected);
            scattered.Origin.CopyFrom(record.Position);
            scattered.Direction.CopyFrom(target);

            return (Vector3F.Dot(scattered.Direction, record.Normal) > 0F);
        }
    }
}
