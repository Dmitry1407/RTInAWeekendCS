using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material
{
    public abstract class Material
    {
        protected Random random = new Random();
        private Vector3F vRandom = new Vector3F();
        protected Vector3F normal = new Vector3F();
        protected Vector3F uv = new Vector3F();
        protected Vector3F reflected = new Vector3F();
        protected Vector3F refracted = new Vector3F();
        protected Vector3F vtmp = new Vector3F();
        protected FColor emitColor = new FColor();
        protected Ray scattered;

        public abstract bool Scatter(Ray rIN, HitRecord record, FColor attenuation, Ray scattered);

        public virtual FColor Emitted(Single u, Single v, Vector3F p) { return emitColor; }

        protected Single Schlick(Single cosine, Single refIDX)
        {
            Single r0 = (1F - refIDX) / (1F + refIDX);
            r0 = r0 * r0;
            return r0 + (1F - r0) * (Single)Math.Pow((1F - cosine), 5F);
        }

        protected bool Refract(Vector3F v, Single ni_over_nt)
        {
            Vector3F.Div(uv, v, v.Length());
            Single dt = Vector3F.Dot(uv, normal);
            Single discriminant = 1F - ni_over_nt * ni_over_nt * (1F - dt * dt);
            if (discriminant > 0F)
            {
                ////refracted = ni_over_nt * (uv - n * dt) - n * (Single)Math.Sqrt(discriminant);
                Vector3F.Mul(refracted, normal, -dt);
                refracted.Add(uv);
                refracted.Mul(ni_over_nt);

                Vector3F.Mul(vtmp, normal, (Single)Math.Sqrt(discriminant));
                refracted.Sub(vtmp);
                return true;
            }
            return false;
        }

        protected Vector3F Reflect(Vector3F v, Vector3F n)
        {
            ////return v - 2F * Vector.Dot(v, n) * n;
            Vector3F.Mul(reflected, n, -2F * Vector3F.Dot(v, n));
            reflected.Add(v);
            return reflected;
        }

        protected Vector3F RandomInUnitSphere()
        {
            Single rx, ry, rz;
            do
            {
                rx = (Single)random.NextDouble() * 2F - 1F;
                ry = (Single)random.NextDouble() * 2F - 1F;
                rz = (Single)random.NextDouble() * 2F - 1F;
            } while ((rx * rx + ry * ry + rz * rz) >= 1F);
            vRandom.X = rx;
            vRandom.Y = ry;
            vRandom.Z = rz;
            return vRandom;
        }
    }
}
