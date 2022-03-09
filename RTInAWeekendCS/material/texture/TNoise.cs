using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material.texture
{
    public class TNoise : Texture
    {
        private FColor color = new FColor();

        public TNoise() { }

        public override FColor GetColor(Single u, Single v, Vector3F p)
        {
            return color;
        }
    }
}
