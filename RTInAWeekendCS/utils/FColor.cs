using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTInAWeekendCS.utils
{
    public class FColor
    {
        private Single r;
        private Single g;
        private Single b;

        public FColor() : this(0F, 0F, 0F) { }

        public FColor(Single r, Single g, Single b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Single R { get { return r; } set { r = value; } }
        public Single G { get { return g; } set { g = value; } }
        public Single B { get { return b; } set { b = value; } }

        public void Reset() { this.r = 0F; this.g = 0F; this.b = 0F; }

        public void CopyFrom(FColor other) { this.r = other.r; this.g = other.g; this.b = other.b; }

        public void SetRGB(Single r, Single g, Single b) { this.r = r; this.g = g; this.b = b; }

        public override string ToString()
        {
            return String.Format("Color [R:{0:F2}] [G:{1:F2}] [B:{2:F2}]", r, g, b);
        }
    }
}
