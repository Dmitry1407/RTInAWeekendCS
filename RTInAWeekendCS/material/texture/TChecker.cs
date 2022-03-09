using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material.texture
{
    public class TChecker : Texture
    {
        private Texture odd;
        private Texture even;

        public TChecker(Texture odd, Texture even)
        {
            this.odd = odd;
            this.even = even;
        }

        public override FColor GetColor(Single u, Single v, Vector3F p)
        {
            Single sin = (Single)(Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z));
            return (sin < 0F) ? odd.GetColor(u, v, p) : even.GetColor(u, v, p);
        }
    }
}
