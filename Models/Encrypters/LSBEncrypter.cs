using System.Drawing;

namespace SteganographyGraduate.Models.Encrypters
{
    [PutInIoC(nameof(LSBEncrypter))]
    public class LSBEncrypter : EncrypterBase
    {
        public LSBEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key) { }

        public override bool CheckCapacity() => MessageToEncrypt.Length <= Rows * Columns / PixelCountLSB;

        protected override int Encrypt(int textIndex)
        {
            var (i, j) = GetRandomPosition();
            var p1 = UnsafeBitmap[i, j];

            var (n, m) = GetRandomPosition();
            var p2 = UnsafeBitmap[n, m];

            var (k, t) = GetRandomPosition();
            var p3 = UnsafeBitmap[k, t];

            (p1, p2, p3) = Encrypt(p1, p2, p3, (byte)MessageToEncrypt[textIndex]);

            UnsafeBitmap[i, j] = p1;
            UnsafeBitmap[n, m] = p2;
            UnsafeBitmap[k, t] = p3;
            var d = MessageToEncrypt[textIndex];
          
            textIndex++;
            return textIndex;
        }

        private (Pixel p1, Pixel p2, Pixel p3) Encrypt(Pixel pix1, Pixel pix2, Pixel pix3, byte symbol)
        {
            var pixel3G = (byte)((pix3.G & 254) | (symbol & 1));
            var pixel3R = (byte)((pix3.R & 254) | ((symbol & 2) >> 1));
            var p3 = new Pixel() { B = pix3.B, G = pixel3G, R = pixel3R };

            var pixel2B = (byte)((pix2.B & 254) | ((symbol & 4) >> 2));
            var pixel2G = (byte)((pix2.G & 254) | ((symbol & 8) >> 3));
            var pixel2R = (byte)((pix2.R & 254) | ((symbol & 16) >> 4));
            var p2 = new Pixel() { B = pixel2B, G = pixel2G, R = pixel2R };

            var pixel1B = (byte)((pix1.B & 254) | ((symbol & 32) >> 5));
            var pixel1G = (byte)((pix1.G & 254) | ((symbol & 64) >> 6));
            var pixel1R = (byte)((pix1.R & 254) | ((symbol & 128) >> 7));
            var p1 = new Pixel() { B = pixel1B, G = pixel1G, R = pixel1R };

            return (p1, p2, p3);
        }
    }
}
