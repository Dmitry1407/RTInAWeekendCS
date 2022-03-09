using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material.texture;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material.light
{
    public class Light : Material
    {
        private Texture emit;

        public Light() : this(new TDiffuse(new FColor(1F, 1F, 1F))) { }

        public Light(Texture emit)
        {
            this.emit = emit;
        }

        public override bool Scatter(Ray rayIN, HitRecord record, FColor attenuation, Ray scattered) { return false; }

        public override FColor Emitted(Single u, Single v, Vector3F p)
        {
            return emit.GetColor(u, v, p);
        }
    }
}
