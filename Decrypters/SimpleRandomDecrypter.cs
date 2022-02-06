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

namespace SteganographyGraduate.Decrypters
{
    [PutInIoC("SimpleRandomDecrypter")]
    public class SimpleRandomDecrypter : DecrypterRandomBase
    {
        public SimpleRandomDecrypter(string imagePath, string key) : base(imagePath, key) { }
    
        public override unsafe IList<byte> Decrypt()
        {
            var (i, j) = GetStartPosition();

            return ImageContainsEncryptInfo(_unsafeBitmap.Bitmap, i, j)
                 ? DecryptImpl()
                 : Array.Empty<byte>();
        }

        private bool ImageContainsEncryptInfo(Bitmap bitmap, int i, int j)
        {
            var pixel = bitmap.GetPixel(i, j);

            return ((Pixel)pixel).Decrypt(null) == MarkingAvailabilityInformation.StartSymbol;
        }

        public unsafe IList<byte> DecryptImpl()
        {
            var bytes = new List<byte>();
            _unsafeBitmap.LockBitmap();

            for (var x = 0; x < _rows; x++)
            {
                for (var y = 0; y < _colls; y++)
                {
                    var i = FindFreePosition(_indexesI);
                    var j = FindFreePosition(_indexesJ);

                    var symbol = _unsafeBitmap[i, j].Decrypt(null);

                    if (symbol == MarkingAvailabilityInformation.EndMetaInfoSymbol)
                    {
                        _unsafeBitmap.UnlockBitmap();
                        return bytes;
                    }

                    bytes.Add((byte)symbol);             
                }
            }

            return bytes;
        }

        protected override (int metaInfoLength, int infoLength) GetEncryptInfoLength() => throw new NotImplementedException();
    }
}
