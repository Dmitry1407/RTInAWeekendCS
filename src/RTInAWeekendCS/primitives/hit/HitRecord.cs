using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.hit
{
    public class HitRecord
    {
        public HitRecord()
        {
            Position = new Vector3F();
            Normal = new Vector3F();
        }

        public Vector3F Position { get; set; }
        public Vector3F Normal { get; set; }
        public Material Material { get; set; }
        public Single U { get; set; }
        public Single V { get; set; }
        public Single T { get; set; }
    }
}
