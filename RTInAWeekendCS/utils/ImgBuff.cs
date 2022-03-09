using System;

namespace RTInAWeekendCS.utils
{
    public class ImgBuff
    {
        private Byte[] buff;
        private Int32 channels;
        private BColor pxColor = new BColor();
        public Int32 Width { get; protected set; }
        public Int32 Height { get; protected set; }

        public ImgBuff(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
            channels = 3;
            buff = new Byte[Width * Height * channels];
        }

        public Byte[] GetBuff()
        {
            return buff;
        }

        public BColor GetPX(Int32 x, Int32 y)
        {
            if (x < Width && y < Height)
            {
                Int32 index = (Width * (Height - y - 1) + x) * channels;
                pxColor.R = buff[index];
                pxColor.G = buff[index + 1];
                pxColor.B = buff[index + 2];
                return pxColor;
            }
            return null;
        }

        public void SetPX(Int32 x, Int32 y, Byte r, Byte g, Byte b)
        {
            // Int32 index = (Width * (Height - y /*- 1*/) + x) * channels;
            Int32 index = (y * Width + x) * channels;
            if (index >= 0 && index < buff.Length)
            {
                buff[index] = r;
                buff[index + 1] = g;
                buff[index + 2] = b;
            }
        }

        public void SetPX(Int32 x, Int32 y, BColor bcolor)
        {
            if (bcolor != null) { SetPX(x, y, bcolor.R, bcolor.G, bcolor.B); }
        }

        public void SetPX(Int32 x, Int32 y, Single r, Single g, Single b)
        {
            Int32 index = (y * Width + x) * channels;
            buff[index] = r > 1F ? Byte.MaxValue : (Byte)(r * 255.99F);
            buff[index + 1] = g > 1F ? Byte.MaxValue : (Byte)(g * 255.99F);
            buff[index + 2] = b > 1F ? Byte.MaxValue : (Byte)(b * 255.99F);
        }

        public void SetPX(Int32 x, Int32 y, FColor fcolor)
        {
            if (fcolor != null) { SetPX(x, y, fcolor.R, fcolor.G, fcolor.B); }
        }

        public Boolean FlipHorizontally()
        {
            if (buff == null || buff.Length == 0) return false;
            Int32 half = Width >> 1;
            for (Int32 i = 0; i < half; i++)
            {
                for (Int32 j = 0; j < Height; j++)
                {
                    BColor c1 = GetPX(i, j);
                    BColor c2 = GetPX(Width - 1 - i, j);
                    SetPX(i, j, c2);
                    SetPX(Width - 1 - i, j, c1);
                }
            }
            return true;
        }

        public Boolean FlipVertically()
        {
            if (buff == null || buff.Length == 0) return false;
            Int32 bytes_per_line = Width * 3;
            Byte[] line = new Byte[bytes_per_line];
            Int32 half = Height >> 1;
            for (Int32 j = 0; j < half; j++)
            {
                Int32 l1 = j * bytes_per_line;
                Int32 l2 = (Height - 1 - j) * bytes_per_line;
                Array.Copy(buff, l1, line, 0, bytes_per_line);
                Array.Copy(buff, l2, buff, l1, bytes_per_line);
                Array.Copy(line, 0, buff, l2, bytes_per_line);
            }
            return true;
        }
    }
}
