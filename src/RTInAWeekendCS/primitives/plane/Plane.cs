using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.plane
{
    public abstract class Plane : Primitive
    {
        public PlaneType PType { get; set; }
        public Vector3F Normal { get; set; }
        public Single K { get; set; }
    }
}
