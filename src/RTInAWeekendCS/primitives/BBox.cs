using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{
    class BBox : Primitive
    {
        public Vector3F PMin { get; set; }
        public Vector3F PMax { get; set; }

        private Single x0, x1;
        private Single y0, y1;
        private Single z0, z1;
        private Single kEpsilon = 0.0001F;

        public BBox(Single x0, Single x1, Single y0, Single y1, Single z0, Single z1)
        {
            this.x0 = x0; this.x1 = x1;
            this.y0 = y0; this.y1 = y1;
            this.z0 = z0; this.z1 = z1;
            Record = new HitRecord();
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            Single ox = ray.Origin.X; Single oy = ray.Origin.Y; Single oz = ray.Origin.Z;
            Single dx = ray.Direction.X; Single dy = ray.Direction.Y; Single dz = ray.Direction.Z;

            Single tx_min, ty_min, tz_min;
            Single tx_max, ty_max, tz_max;

            Single a = 1F / dx;
            if (a >= 0) { tx_min = (x0 - ox) * a; tx_max = (x1 - ox) * a; }
            else { tx_min = (x1 - ox) * a; tx_max = (x0 - ox) * a; }

            Single b = 1F / dy;
            if (b >= 0) { ty_min = (y0 - oy) * b; ty_max = (y1 - oy) * b; }
            else { ty_min = (y1 - oy) * b; ty_max = (y0 - oy) * b; }

            Single c = 1F / dz;
            if (c >= 0) { tz_min = (z0 - oz) * c; tz_max = (z1 - oz) * c; }
            else { tz_min = (z1 - oz) * c; tz_max = (z0 - oz) * c; }

            Single t0, t1;

            // find largest entering t value
            if (tx_min > ty_min) { t0 = tx_min; }
            else { t0 = ty_min; }
            if (tz_min > t0) { t0 = tz_min; }

            // find smallest exiting t value
            if (tx_max < ty_max) { t1 = tx_max; }
            else { t1 = ty_max; }
            if (tz_max < t1) { t1 = tz_max; }

            return (t0 < t1 && t1 > kEpsilon) ? Record : null;
        }
    }
}
