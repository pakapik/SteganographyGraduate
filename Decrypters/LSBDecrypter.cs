using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Decrypters
{
    [PutInIoC("LSBDecrypter")]
    public class LSBDecrypter : DecrypterBase
    {
        public LSBDecrypter(string imagePath, string key) : base(imagePath, key) { }

        public override IList<byte> Decrypt()
        {
            var textIndex = 0;
            var (metaInfoLength, textLength) = GetEncryptInfoLength();
            var st = "";
            var bytes = new byte[textLength];
            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                var y = x == 0 ? metaInfoLength : 1;
                for (; y < _colls; y += 3)
                {
                    if (textIndex == textLength || y + 1 >= _colls)
                    {
                        break;
                    }

                    var p1 = _unsafeBitmap[x, y - 1];
                    var p2 = _unsafeBitmap[x, y];
                    var p3 = _unsafeBitmap[x, y + 1];
                    if(textIndex == 195)
                    {
                        var d = 0;
                    }
                    st += (char)Decrypt(p1, p2, p3);
                    bytes[textIndex] = Decrypt(p1, p2, p3);
                    textIndex++;
                }
            }

            _unsafeBitmap.UnlockBitmap();
            return bytes;
        }

        protected override (int metaInfoLength, int infoLength) GetEncryptInfoLength()
        {
            // NOTE: итерация идет по строке в ширину без проверки выхода за границу
            // Так же если в картинке случайно окажется символ '\0',
            // то прога крашнется на парсинге строки.

            var i = StegoHelper.PixelCountToEcnryptLSB;
            var info = string.Empty;
            char symbol;

            _unsafeBitmap.LockBitmap();

            while (true)
            {
                var pixel1 = _unsafeBitmap[0, i++];
                var pixel2 = _unsafeBitmap[0, i++];
                var pixel3 = _unsafeBitmap[0, i++];

                symbol = (char)Decrypt(pixel1, pixel2, pixel3);

                if (symbol == MarkingAvailabilityInformation.EndMetaInfoSymbol)
                {
                    break;
                }

                info += symbol;
            }

            _unsafeBitmap.UnlockBitmap();
            // + 2, becouse exist Start and End symbols 
            // * 3, becouse one symbol (byte) = 3 Pixel
            // + 1, becouse ([0, 15], [0, 16], [0, 17]) in upper Alg. Not ([0, 14], [0, 15], [0, 16])
            return (((info.Length + 2) * 3) + 1, int.Parse(info));
        }

        private byte Decrypt(Pixel p1, Pixel p2, Pixel p3)
        {
            var bitR = (p1.R & 1) << 7;
            var bitG = (p1.G & 1) << 6;
            var bitB = (p1.B & 1) << 5;

            var result = bitR | bitG | bitB;

            bitR = (p2.R & 1) << 4;
            bitG = (p2.G & 1) << 3;
            bitB = (p2.B & 1) << 2;

            result |= bitR | bitG | bitB;

            bitR = (p3.R & 1) << 1;
            bitG = p3.G & 1;

            result |= bitR | bitG;

            return (byte)result;
        }
    }
}
