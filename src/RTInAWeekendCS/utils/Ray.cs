using System;

namespace RTInAWeekendCS.utils
{
    public class Ray
    {
        private Vector3F point;
        public Vector3F Origin { get; set; }
        public Vector3F Direction { get; set; }

        public Ray()
        {
            point = new Vector3F();
            Origin = new Vector3F();
            Direction = new Vector3F();
        }

        public Ray(Vector3F origin, Vector3F direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        public Vector3F PointAtParameter(Single t)
        {
            Vector3F.Mul(point, Direction, t);
            point.Add(Origin);
            return point;
        }
    }
}
