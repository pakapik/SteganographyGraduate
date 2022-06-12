using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public struct Pixel : IEquatable<Pixel>
    {
        // Порядок важен, т.к. в памяти пиксель представлен как BGR + Alpha
        // Доступ к памяти:
        // image[offset + 0]  == Blue  0-255
        // image[offset + 1]  == Green 0-255
        // image[offset + 2]  == Red   0-255
        // image[offset + 3]  == Alpha 0-255
        public byte B { get; set; }
        public byte G { get; set; }
        public byte R { get; set; }

        public Pixel(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public bool Equals(Pixel other) => B == other.B && G == other.G && R == other.R;

        public override bool Equals(object obj) => obj is Pixel pixel && Equals(pixel);

        public override int GetHashCode() => HashCode.Combine(B, G, R);

        public override string ToString() => $"R: {R}, G: {G}, B: {B}";

        public static implicit operator Pixel(Color color) => new Pixel { R = color.R, G = color.G, B = color.B };

        public static explicit operator Color(Pixel pixel) => Color.FromArgb(pixel.R, pixel.G, pixel.B);

        public static bool operator == (Pixel thisObj, Pixel other) => thisObj.Equals(other);

        public static bool operator != (Pixel thisObj, Pixel other) => !thisObj.Equals(other);
    }
}
