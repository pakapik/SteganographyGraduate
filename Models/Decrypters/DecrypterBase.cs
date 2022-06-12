using System;
using System.Collections.Generic;

namespace SteganographyGraduate.Models.Decrypters
{
    public abstract class DecrypterBase : CrypterBase, IDecrypter
    {
        public DecrypterBase(string imagePath, string key) : base(imagePath, key) { }

        public unsafe IList<byte> Decrypt()
        {
            var bytes = new List<byte>();
            UnsafeBitmap.LockBitmap();

            var count = Rows * Columns;
            for (var i = 0; i < count; i++)
            {
                var symbol = DecryptImpl();

                if (symbol == MarkingAvailabilityInformation.EndMetaInfoSymbol)
                {
                    UnsafeBitmap.UnlockBitmap();
                    return bytes;
                }

                bytes.Add((byte)symbol);
            }

            return bytes;
        }

        protected abstract unsafe char DecryptImpl();
    }
}
