using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.hit
{
    public abstract class Hitable
    {
        public abstract HitRecord Hit(Ray ray, Single tMin, Single tMax);
    }
}
