using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteganographyGraduate;

namespace SteganographyGraduate.Encrypters
{
    [PutInIoC("SimpleEncrypter")]
    public class SimpleEncrypter : EncrypterBase
    {
        public SimpleEncrypter(string imagePath, string messagePath, string key) : base (imagePath, messagePath, key)
        {
            _messageToEncrypt = GetMessageToEcnrypt(messagePath);
        }

        public override unsafe Bitmap Encrypt()
        {
            var index = 0;
            var length = _messageToEncrypt.Length;
            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                for (var y = 0; y < _colls; y++)
                {
                    if (index == length)
                    {
                        break;
                    }

                    var pixel = _unsafeBitmap[x, y];
                    pixel = pixel.Encrypt((byte)_messageToEncrypt[index], _key);
                    _unsafeBitmap[x, y] = pixel;
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
    }
}
