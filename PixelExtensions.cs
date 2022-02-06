using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public static class PixelExtensions
    {
        public static Color ColorFromArgb(this Pixel pixel) => Color.FromArgb(pixel.R, pixel.G, pixel.B);

        public static char Decrypt(this Pixel pixel, string key)
        {
            var r = pixel.R & 7;
            var g = (pixel.G & 3) << 3;
            var b = (pixel.B & 7) << 5;

            var result = b | g | r;
            var cresult = (char)result;

            return (char)result;
        }

        public static Pixel Encrypt(this Pixel pixel, byte symbol, string key)
        {
            var r = (byte)((pixel.R & 248) | (symbol & 7));
            var g = (byte)((pixel.G & 252) | ((symbol & 24) >> 3));
            var b = (byte)((pixel.B & 248) | ((symbol & 224) >> 5));

            return new Pixel()
            {
                R = r,
                G = g,
                B = b
            };
        }
    }
}
