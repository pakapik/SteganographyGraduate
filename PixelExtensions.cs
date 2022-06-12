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
        public static Color ColorFromArgb(this Pixel pixel) => Color.FromArgb(pixel.B, pixel.G, pixel.R);
    }
}
