using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material
{
    public class Glass : Material
    {
        private Single refIDX;
        private FColor attenuation;

        public Glass(Single refIDX)
        {
            this.refIDX = refIDX;
            this.attenuation = new FColor(1F, 1F, 1F);
        }

        public override bool Scatter(Ray rayIN, HitRecord record, FColor attenuation, Ray scattered)
        {
            Reflect(rayIN.Direction, record.Normal);
            Single ni_over_nt;
            ////attenuation = this.attenuation;
            attenuation.CopyFrom(this.attenuation);
            Single reflectProb;
            Single cosine;

            if (Vector3F.Dot(rayIN.Direction, record.Normal) > 0F)
            {
                Vector3F.Mul(normal, record.Normal, -1F);
                ni_over_nt = refIDX;
                // cosine = ref_idx * dot(r_in.direction(), rec.normal) / r_in.direction().length();
                cosine = Vector3F.Dot(rayIN.Direction, record.Normal) / rayIN.Direction.Length();
                cosine = (Single)Math.Sqrt(1F - refIDX * refIDX * (1F - cosine * cosine));
            }
            else
            {
                normal.CopyFrom(record.Normal);
                ni_over_nt = 1F / refIDX;
                cosine = -Vector3F.Dot(rayIN.Direction, record.Normal) / rayIN.Direction.Length();
            }

            if (Refract(rayIN.Direction, ni_over_nt))
            {
                reflectProb = Schlick(cosine, refIDX);
            }
            else { reflectProb = 1F; }

            scattered.Origin.CopyFrom(record.Position);
            scattered.Direction.CopyFrom((Single)random.NextDouble() < reflectProb ? reflected : refracted);
            return true;
        }
    }
}
