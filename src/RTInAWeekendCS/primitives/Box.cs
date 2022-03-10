using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.plane;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{
    public class Box : Primitive
    {
        public Vector3F PMin { get; set; }
        public Vector3F PMax { get; set; }

        private Single x0, x1;
        private Single y0, y1;
        private Single z0, z1;
        private Single kEpsilon = 0.0001F;

        private Vector3F normalXN = new Vector3F(-1F, 0F, 0F);	// -x face
        private Vector3F normalYN = new Vector3F(0F, -1F, 0F);	// -y face
        private Vector3F normalZN = new Vector3F(0F, 0F, -1F);	// -z face
        private Vector3F normalXP = new Vector3F(1F, 0F, 0F);	// +x face
        private Vector3F normalYP = new Vector3F(0F, 1F, 0F);	// +y face
        private Vector3F normalZP = new Vector3F(0F, 0F, 1F);	// +z face
        private Vector3F defNormal = new Vector3F(-999F, -999F, -999F);	// default normal

        private Hitable box;

        public Box(Vector3F p0, Vector3F p1, Material material)
        {
            PMin = p0;
            PMax = p1;
            Hitable[] list = new Hitable[6];
            Plane plane1 = new XYPlane(p0.X, p1.X, p0.Y, p1.Y, p1.Z, material);
            Plane plane2 = new XYPlane(p0.X, p1.X, p0.Y, p1.Y, p0.Z, material);
            plane2.FlipNormales = true;
            Plane plane3 = new XZPlane(p0.X, p1.X, p0.Z, p1.Z, p1.Y, material);
            Plane plane4 = new XZPlane(p0.X, p1.X, p0.Z, p1.Z, p0.Y, material);
            plane4.FlipNormales = true;
            Plane plane5 = new YZPlane(p0.Y, p1.Y, p0.Z, p1.Z, p1.X, material);
            Plane plane6 = new YZPlane(p0.Y, p1.Y, p0.Z, p1.Z, p0.X, material);
            plane6.FlipNormales = true;

            list[0] = plane1;
            list[1] = plane2;
            list[2] = plane3;
            list[3] = plane4;
            list[4] = plane5;
            list[5] = plane6;
            box = new HitList(list, 6);
        }

        ////public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        ////{
        ////    return box.Hit(ray, tMin, tMax);
        ////}

        public Box(Single x0, Single x1, Single y0, Single y1, Single z0, Single z1, Material material)
        {
            this.x0 = x0; this.x1 = x1;
            this.y0 = y0; this.y1 = y1;
            this.z0 = z0; this.z1 = z1;
            Material = material;
            Record = new HitRecord();
            Record.Material = Material;
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
            int face_in, face_out;

            // find largest entering t value
            if (tx_min > ty_min) { t0 = tx_min; face_in = (a >= 0.0) ? 0 : 3; }
            else { t0 = ty_min; face_in = (b >= 0.0) ? 1 : 4; }

            if (tz_min > t0) { t0 = tz_min; face_in = (c >= 0.0) ? 2 : 5; }

            // find smallest exiting t value
            if (tx_max < ty_max) { t1 = tx_max; face_out = (a >= 0.0) ? 3 : 0; }
            else { t1 = ty_max; face_out = (b >= 0.0) ? 4 : 1; }

            if (tz_max < t1) { t1 = tz_max; face_out = (c >= 0.0) ? 5 : 2; }
            if (t0 < t1 && t1 > kEpsilon)
            {  // condition for a hit
                if (t0 > kEpsilon)
                {
                    tMin = t0; // ray hits outside surface
                }
                else
                {
                    tMin = t1; // ray hits inside surface
                }

                Record.T = tMin;
                Record.Position = ray.PointAtParameter(tMin);
                Record.Normal = GetNormal(face_out);
                return (Record);
            }

            return (null);
        }

        private Vector3F GetNormal(Int32 face_hit)
        {
            switch (face_hit)
            {
                case 0: return normalXN;	// -x face
                case 1: return normalYN;	// -y face
                case 2: return normalZN;	// -z face
                case 3: return normalXP;	// +x face
                case 4: return normalYP;	// +y face
                case 5: return normalZP;	// +z face
                //case 0: return normalXP;	// -x face
                //case 1: return normalYP;	// -y face
                //case 2: return normalZP;	// -z face
                //case 3: return normalXN;	// +x face
                //case 4: return normalYN;	// +y face
                //case 5: return normalZN;	// +z face
                default: return defNormal;
            }
        }
    }
}
