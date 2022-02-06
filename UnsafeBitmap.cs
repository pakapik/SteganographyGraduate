using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public unsafe class UnsafeBitmap
    {
        private int _width;
        private BitmapData _bitmapData = null;
        private byte* _pBase = null;

        private readonly Bitmap _bitmap;
        private readonly int _sizeOfPixelData = sizeof(Pixel);

        public Bitmap Bitmap => _bitmap;

        public int Width => _bitmap.Width;
        public int Height => _bitmap.Height;

        public Pixel this[int x, int y]
        {
            get => *PixelAt(x, y);
            set => SetPixel(x, y, value);
        }

        public UnsafeBitmap(Bitmap bitmap) => _bitmap = new Bitmap(bitmap);

        public UnsafeBitmap(string path) => _bitmap = new Bitmap(path);

        public UnsafeBitmap(int width, int height) => _bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        public void Dispose() => _bitmap.Dispose();

        public void LockBitmap()
        {
            var unit = GraphicsUnit.Pixel;
            var boundsF = _bitmap.GetBounds(ref unit);
            var bounds = new Rectangle((int)boundsF.X,
                                       (int)boundsF.Y,
                                       (int)boundsF.Width,
                                       (int)boundsF.Height);

            _width = (int)boundsF.Width * sizeof(Pixel);

            if (_width % 4 != 0)
            {
                _width = 4 * ((_width / 4) + 1);
            }

            _bitmapData = _bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            _pBase = (byte*)_bitmapData.Scan0.ToPointer();
        }

        public void UnlockBitmap()
        {
            _bitmap.UnlockBits(_bitmapData);
            _bitmapData = null;
            _pBase = null;
        }

        public Pixel GetPixel(int x, int y) => *PixelAt(x, y);

        public void SetPixel(int x, int y, Pixel color)
        {
            Pixel* pixel = PixelAt(x, y);
            *pixel = color;
        }

        private Pixel* PixelAt(int x, int y) => (Pixel*)(_pBase + (y * _width) + (x * _sizeOfPixelData));
    }
}

