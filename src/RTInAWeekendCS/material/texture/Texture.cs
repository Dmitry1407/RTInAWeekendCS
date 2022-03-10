using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.material.texture
{
    public abstract class Texture
    {
        public abstract FColor GetColor(Single u, Single v, Vector3F p);
    }
}
