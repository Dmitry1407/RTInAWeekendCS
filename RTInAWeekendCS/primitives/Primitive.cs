using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{
    public abstract class Primitive : Hitable
    {
        public HitRecord Record { get; set; }
        public Vector3F Center { get; set; }
        public Single Radius { get; set; }
        public Material Material { get; set; }
        public Boolean FlipNormales { get; set; }
    }
}
