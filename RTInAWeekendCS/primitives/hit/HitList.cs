using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives.hit
{
    public class HitList : Hitable
    {
        private Hitable[] list;
        private Int32 size;

        public HitList() { }

        public HitList(Hitable[] list, Int32 n)
        {
            this.list = list;
            this.size = n;
        }

        public Hitable[] GetList()
        {
            return list;
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            HitRecord record = null;
            HitRecord hitAnything = null;
            Single closest_so_far = tMax;

            for (Int32 i = 0; i < size; i++)
            {
                hitAnything = list[i].Hit(ray, tMin, closest_so_far);
                if (hitAnything != null)
                {
                    closest_so_far = hitAnything.T;
                    record = hitAnything;
                }
            }
            return record;
        }
    }
}
