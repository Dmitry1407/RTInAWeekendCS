using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material.texture
{
    public class TDiffuse : Texture
    {
        private FColor color;

        public TDiffuse() : this(new FColor()) { }

        public TDiffuse(FColor color)
        {
            this.color = color;
        }

        public override FColor GetColor(Single u, Single v, Vector3F p)
        {
            return color;
        }
    }
}
