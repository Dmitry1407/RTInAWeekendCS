using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS
{
    public class Camera
    {
        private Random random = new Random();
        private Vector3F vRandom = new Vector3F();
        private Vector3F offsetX = new Vector3F();
        private Vector3F offsetY = new Vector3F();
        private Vector3F offset = new Vector3F();
        private Vector3F direction = new Vector3F();
        private Vector3F vtmp;

        private Ray ray;

        private Vector3F origin;
        private Vector3F lowerLeftCorner;
        private Vector3F horizontal;
        private Vector3F vertical;
        private Vector3F u, v, w;
        private Single lensRadius;

        public Camera(Vector3F lookFrom, Vector3F lookAt, Vector3F vup, Single vfov, Single aspect, Single aperture, Single focusDist)
        {
            ray = new Ray();

            // vfov is top to bottom in degrees
            lensRadius = aperture / 2F;
            Single theta = (Single)(vfov * Math.PI / 180);
            Single halfHeight = (Single)Math.Tan(theta / 2F);
            Single halfWidth = aspect * halfHeight;
            origin = lookFrom;

            ////w = Vector.unitVector(lookFrom - lookAt);
            vtmp = new Vector3F(lookFrom.X, lookFrom.Y, lookFrom.Z);
            vtmp.Sub(lookAt);
            w = Vector3F.UnitVector(vtmp);
            u = Vector3F.UnitVector(Vector3F.Cross(vup, w));
            v = Vector3F.Cross(w, u);

            ////lowerLeftCorner = origin - halfWidth * focusDist * u - halfHeight * focusDist * v - focusDist * w;
            lowerLeftCorner = new Vector3F(origin.X, origin.Y, origin.Z);
            lowerLeftCorner.Sub(halfWidth * focusDist * u);
            lowerLeftCorner.Sub(halfHeight * focusDist * v);
            lowerLeftCorner.Sub(focusDist * w);

            horizontal = new Vector3F(u.X, u.Y, u.Z);
            horizontal.Mul(2F * halfWidth * focusDist);

            vertical = new Vector3F(v.X, v.Y, v.Z);
            vertical.Mul(2F * halfHeight * focusDist);
        }

        public Ray GetSingleRay(Single s, Single t)
        {
            ////return new Ray(origin, lowerLeftCorner + s * horizontal + t * vertical - origin );
            Vector3F.Mul(vtmp, horizontal, s);
            Vector3F.Add(direction, lowerLeftCorner, vtmp);
            Vector3F.Mul(vtmp, vertical, t);
            direction.Add(vtmp);
            direction.Sub(origin);

            ray.Origin.CopyFrom(origin);
            ray.Direction.CopyFrom(direction);
            return ray;
        }

        public Ray GetAARay(Single s, Single t)
        {
            Vector3F rand = RandomInUnitDisk();
            rand.Mul(lensRadius);

            ////Vector offset = u * rand.X + v * rand.Y;
            Vector3F.Mul(offsetX, u, rand.X);
            Vector3F.Mul(offsetY, v, rand.Y);
            Vector3F.Add(offset, offsetX, offsetY);
            offset.Add(origin);

            ////return new Ray(origin + offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset);
            Vector3F.Mul(vtmp, horizontal, s);
            Vector3F.Add(direction, lowerLeftCorner, vtmp);
            Vector3F.Mul(vtmp, vertical, t);
            direction.Add(vtmp);
            direction.Sub(offset);

            ray.Origin.CopyFrom(offset);
            ray.Direction.CopyFrom(direction);
            return ray;
        }

        public Vector3F RandomInUnitDisk()
        {
            Single rx, ry;
            do
            {
                rx = (Single)random.NextDouble() * 2F - 1F;
                ry = (Single)random.NextDouble() * 2F - 1F;
            } while ((rx * rx + ry * ry) >= 1F);
            vRandom.X = rx;
            vRandom.Y = ry;
            return vRandom;
        }
    }
}
