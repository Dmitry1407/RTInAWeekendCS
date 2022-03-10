using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material.texture;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material
{
    public class Lambertian : Material
    {
        private Texture albedo;
        private Vector3F target = new Vector3F();

        public Lambertian(Texture albedo)
        {
            this.albedo = albedo;
        }

        public override bool Scatter(Ray rayIN, HitRecord record, FColor attenuation, Ray scattered)
        {
            Vector3F.Add(target, record.Normal, RandomInUnitSphere());
            scattered.Origin.CopyFrom(record.Position);
            scattered.Direction.CopyFrom(target);
            ////attenuation = albedo.GetColor(record.U, record.V, record.Position);
            attenuation.CopyFrom(albedo.GetColor(record.U, record.V, record.Position));
            return true;
        }
    }
}
