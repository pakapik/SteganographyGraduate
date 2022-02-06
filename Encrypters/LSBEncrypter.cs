using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace SteganographyGraduate.Encrypters
{
    [PutInIoC("LSBEncrypter")]
    public class LSBEncrypter : EncrypterBase
    {
        public LSBEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key)
        {
            _messageToEncrypt = GetMessageToEcnrypt(messagePath);
        }

        public override Bitmap Encrypt() => string.IsNullOrEmpty(_key) ? EncryptWithoutKey() : EncryptWithKey();

        private Bitmap EncryptWithoutKey()
        {
            var index = 0;
            var length = _messageToEncrypt.Length;

            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                for (var y = 1; y < _colls; y += 3)
                {
                    if (index == length || y + 1 >= _colls)
                    {
                        break;
                    }

                    var p1 = _unsafeBitmap[x, y - 1];
                    var p2 = _unsafeBitmap[x, y];
                    var p3 = _unsafeBitmap[x, y + 1];

                    (p1, p2, p3) = Encrypt(p1, p2, p3, (byte)_messageToEncrypt[index]);

                    _unsafeBitmap[x, y - 1] = p1;
                    _unsafeBitmap[x, y] = p2;
                    _unsafeBitmap[x, y + 1] = p3;

                    index++;
                }
            }

            _unsafeBitmap.UnlockBitmap();

            return _unsafeBitmap.Bitmap;
        }

        protected override string GetMessageToEcnrypt(string messagePath)
        {
            var message = ReadMessageFromFile(messagePath);
          
            return $"{MarkingAvailabilityInformation.StartSymbol}" +
                   $"{message.Length}" +
                   $"{MarkingAvailabilityInformation.EndMetaInfoSymbol}" +
                   $"{message}";
        }

        private (Pixel p1, Pixel p2, Pixel p3) Encrypt(Pixel pix1, Pixel pix2, Pixel pix3, byte symbol)
        {
            var pixel3G = (byte)((pix3.G & 254) | (symbol & 1));
            var pixel3R = (byte)((pix3.R & 254) | ((symbol & 2) >> 1));
            var p3 = new Pixel() { R = pixel3R, G = pixel3G, B = pix3.B };

            var pixel2B = (byte)((pix2.B & 254) | ((symbol & 4) >> 2));
            var pixel2G = (byte)((pix2.G & 254) | ((symbol & 8) >> 3));
            var pixel2R = (byte)((pix2.R & 254) | ((symbol & 16) >> 4));
            var p2 = new Pixel() { R = pixel2R, G = pixel2G, B = pixel2B };

            var pixel1B = (byte)((pix1.B & 254) | ((symbol & 32) >> 5));
            var pixel1G = (byte)((pix1.G & 254) | ((symbol & 64) >> 6));
            var pixel1R = (byte)((pix1.R & 254) | ((symbol & 128) >> 7));
            var p1 = new Pixel() { R = pixel1R, G = pixel1G, B = pixel1B };

            return (p1, p2, p3);
        }

        private Bitmap EncryptWithKey()
        {
            return null;
        }
    }
}
