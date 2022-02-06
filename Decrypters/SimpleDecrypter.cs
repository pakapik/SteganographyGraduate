using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteganographyGraduate;

// TODO: сделать фабрику/DI для крипторов/декрипторов
// NOTE: Lock/Unlock всегда в паре идут

namespace SteganographyGraduate.Decrypters
{
    [PutInIoC("SimpleDecrypter")]
    public class SimpleDecrypter : DecrypterBase
    {
        public SimpleDecrypter(string imagePath, string key) : base(imagePath, key) { }

        public override unsafe IList<byte> Decrypt()
        {
            return ImageContainsEncryptInfo(_unsafeBitmap.Bitmap) 
                 ? DecryptImpl()
                 : Array.Empty<byte>();
        }

        private bool ImageContainsEncryptInfo(Bitmap bitmap)
        {
            var color = bitmap.GetPixel(0, 0);
            var pixel = (Pixel)color;

            return pixel.Decrypt(null) == MarkingAvailabilityInformation.StartSymbol;
        }

        private unsafe byte[] DecryptImpl()
        {         
            var textIndex = 0;
            var (metaInfoLength, textLength) = GetEncryptInfoLength();
            var bytes = new byte[textLength];

            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                var y = x == 0 ? metaInfoLength : 0;
                for (; y < _colls; y++)
                {
                    if (textIndex == textLength)
                    {
                        break;
                    }

                    var pixel = _unsafeBitmap.GetPixel(x, y);
                    bytes[textIndex] = (byte)pixel.Decrypt(null);
                    textIndex++;
                }
            }

            _unsafeBitmap.UnlockBitmap();

            return bytes;
        }

        protected override (int metaInfoLength, int infoLength) GetEncryptInfoLength()
        {
            var i = 1;
            var info = string.Empty;          
            char symbol;

            _unsafeBitmap.LockBitmap();

            while (true)
            {
                symbol = _unsafeBitmap.GetPixel(0, i).Decrypt(null);

                if (symbol == MarkingAvailabilityInformation.EndMetaInfoSymbol)
                {
                    break;
                }

                info += symbol;
                i++;
            }

            _unsafeBitmap.UnlockBitmap();
            // + 2, becouse exist Start and End symbols 
            return (info.Length + 2, int.Parse(info));
        }
    }
}
