using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public struct Pixel : IEquatable<Pixel>
    {
        public byte R;
        public byte G;
        public byte B;

        public bool Equals(Pixel other) => R == other.R && G == other.G && B == other.B;

        public override string ToString() => $"R: {R}, G: {G}, B: {B}";

        public static implicit operator Pixel(Color color) => new Pixel { R = color.R, G = color.G, B = color.B };       
    }

    public static class BitmapHelper
    {
        public static Color[,] GetPixelsEx(this Bitmap bmp) => ProcessBitmap(bmp, pxl => Color.FromArgb(pxl.R, pxl.G, pxl.B));

        public static float[,] GetBrightnessEx(this Bitmap bmp) => ProcessBitmap(bmp, pxl => Color.FromArgb(pxl.R, pxl.G, pxl.B).GetBrightness()); 
        
        public static unsafe T[,] ProcessBitmap<T>(this Bitmap bitmap, Func<Pixel, T> func)
        {
            var lockBits = bitmap.LockBits(new Rectangle(0, 
                                                              0,
                                                              bitmap.Width, 
                                                              bitmap.Height),
                                           ImageLockMode.ReadOnly,
                                           bitmap.PixelFormat);

            var padding = lockBits.Stride - (bitmap.Width * sizeof(Pixel));

            var width = bitmap.Width;
            var height = bitmap.Height;

            var result = new T[height, width];

            var ptr = (byte*)lockBits.Scan0;
            var pixelSize = sizeof(Pixel);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var pixel = (Pixel*)ptr;
                    result[i, j] = func(*pixel);
                    ptr += pixelSize;
                }
                ptr += padding;
            }

            bitmap.UnlockBits(lockBits);

            return result;
        }


        public static unsafe void SetPixelEx(this Bitmap bitmap, Color[,] colors)
        {
            var lockBits = bitmap.LockBits(new Rectangle(0,
                                                              0,
                                                              bitmap.Width,
                                                              bitmap.Height),
                                           ImageLockMode.ReadOnly,
                                           bitmap.PixelFormat);

            var padding = lockBits.Stride - (bitmap.Width * sizeof(Pixel));

            var width = bitmap.Width;
            var height = bitmap.Height;

            var result = new Color[height, width];

            var ptr = (byte*)lockBits.Scan0;
            var pixelSize = sizeof(Pixel);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var pixel = (Pixel*)ptr;
                    result[i, j] = colors[i, j];
                    ptr += pixelSize;
                }
                ptr += padding;
            }

            bitmap.UnlockBits(lockBits);
        }
    }
}
