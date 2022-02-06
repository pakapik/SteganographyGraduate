using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteganographyGraduate;

namespace SteganographyGraduate.Encrypters
{
    [PutInIoC("SimpleRandomEncrypter")]
    public class SimpleRandomEncrypter : EncrypterRandomBase
    {
        // NOTE: rnd.Next(min, max);  // min включается, max не включается.

        public SimpleRandomEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key)
        {
            _messageToEncrypt = GetMessageToEcnrypt(messagePath);
        }

        public override unsafe Bitmap Encrypt()
        {
            var textIndex = 0;
            var textLength = _messageToEncrypt.Length;

            var (i, j) = GetStartPosition();
            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                for (var y = 0; y < _colls; y++)
                {
                    if (textIndex == textLength)
                    {
                        break;
                    }

                    var pixel = _unsafeBitmap[i, j];
                    pixel = pixel.Encrypt((byte)_messageToEncrypt[textIndex], _key);
                    _unsafeBitmap[i, j] = pixel;
                    textIndex++;

                    i = FindFreePosition(_indexesI);
                    j = FindFreePosition(_indexesJ);
                }
            }

            _unsafeBitmap.UnlockBitmap();

            return _unsafeBitmap.Bitmap;
        }

        protected override string GetMessageToEcnrypt(string messagePath)
        {
            var message = ReadMessageFromFile(messagePath);

            return $"{MarkingAvailabilityInformation.StartSymbol}" +
                   $"{message}" +
                   $"{MarkingAvailabilityInformation.EndMetaInfoSymbol}";
        }
    }
}
