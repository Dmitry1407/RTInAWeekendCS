using System;

namespace RTInAWeekendCS.utils
{
    public class BColor
    {
        private Byte r;
        private Byte g;
        private Byte b;

        public BColor() : this(0, 0, 0) { }

        public BColor(Byte r, Byte g, Byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Byte R { get { return r; } set { r = value; } }
        public Byte G { get { return g; } set { g = value; } }
        public Byte B { get { return b; } set { b = value; } }

        public void Reset() { this.r = 0; this.g = 0; this.b = 0; }

        public void CopyFrom(BColor other) { this.r = other.r; this.g = other.g; this.b = other.b; }

        public void SetRGB(Byte r, Byte g, Byte b) { this.r = r; this.g = g; this.b = b; }

        public override string ToString()
        {
            return String.Format("BColor [R:{0}] [G:{1}] [B:{2}]", r, g, b);
        }
    }
}
