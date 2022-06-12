using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;

namespace SteganographyGraduate.Models.Encrypters
{
    [PutInIoC(nameof(CJBEncrypter))]
    public class CJBEncrypter : EncrypterBase
    {
        private const double Lymbda = 0.2;
        private const int Theta = 20;
        public const double CoefficientR = 0.29890;  /*0.2126;*/
        public const double CoefficientG = 0.58662;  /*0.7152;*/
        public const double CoefficientB = 0.11448;  /*0.0722;*/
        
        public CJBEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key) { }

        protected override int Encrypt(int messageSymbolIndex)
        {
            var symbol = MessageToEncrypt[messageSymbolIndex];

            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < Theta; j++)
                {
                    var (x, y) = GetRandomPosition();
                    var pixel = UnsafeBitmap[x, y];

                    var brightness = (CoefficientR * pixel.R) + (CoefficientG * pixel.G) + (CoefficientB * pixel.B);
                    var blueChannel = pixel.B + (2 * (symbol & 1) - 1) * Lymbda * brightness;
                    var blue = GetNormalizeBlueChannel(blueChannel);
                    pixel.B = blue;

                    UnsafeBitmap[x, y] = pixel;
                }

                symbol >>= 1;
            }

            messageSymbolIndex++;
            return messageSymbolIndex;
        }

        private byte GetNormalizeBlueChannel(double blueChannel)
        {
            var blue = Math.Round(blueChannel);

            if (blue > 255)
            {
                blue = 255;
            }

            if (blue < 0)
            {
                blue = 0;
            }

            return (byte)blue;
        }

        protected override (int i, int j) GetRandomPosition()
        {
            var lengthI = IndexesI.Length - PixelCountCJB;
            var lengthJ = IndexesJ.Length - PixelCountCJB;

            var minBorder = PixelCountCJB; 

            var i = Rnd.Next(minBorder, lengthI);
            var j = Rnd.Next(minBorder, lengthJ);

            while (_coordinates.Contains((i, j)))
            {
                i = Rnd.Next(minBorder, lengthI);
                j = Rnd.Next(minBorder, lengthJ);
            }

            _coordinates.Add((i, j));
            for (var index = 1; index <= PixelCountCJB; index++)
            {
                _coordinates.Add((i + index, j));
                _coordinates.Add((i - index, j));
                _coordinates.Add((i, j + index));
                _coordinates.Add((i, j - index));
            }

            return (i, j);
        }

        public override bool CheckCapacity()
        {
            return true; // TODO: тут вероятностный характер внедрения
        }
    }
}
