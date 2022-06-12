using System.Drawing;

namespace SteganographyGraduate.Models.Encrypters
{
    [PutInIoC(nameof(RLBEncrypter))]
    public class RLBEncrypter : EncrypterBase
    {
        public RLBEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key) { }

        public override bool CheckCapacity() => MessageToEncrypt.Length <= Rows * Columns;

        protected override unsafe int Encrypt(int textIndex)
        {
            var (i, j) = GetRandomPosition();
            var pixel = UnsafeBitmap[i, j];
            pixel = EncryptSymbol(pixel, (byte)MessageToEncrypt[textIndex]);
            UnsafeBitmap[i, j] = pixel;

            textIndex++;
            return textIndex;
        }

        private Pixel EncryptSymbol(Pixel pixel, byte symbol)
        {
            var b = (byte)((pixel.B & 248) | (symbol & 7));
            var g = (byte)((pixel.G & 252) | ((symbol & 24) >> 3));
            var r = (byte)((pixel.R & 248) | ((symbol & 224) >> 5));

            return new Pixel()
            {
                B = b,
                G = g,
                R = r
            };
        }

        protected override string GetMessageToEcnrypt(string messagePath)
        {
            var message = ReadMessageFromFile(messagePath);

            return $"{message}" +
                   $"{MarkingAvailabilityInformation.EndMetaInfoSymbol}";
        }  
    }
}
