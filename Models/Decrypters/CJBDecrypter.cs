using System.Collections.Generic;
using System.Linq;

namespace SteganographyGraduate.Models.Decrypters
{
    [PutInIoC(nameof(CJBDecrypter))]
    public class CJBDecrypter : DecrypterBase
    {
        private readonly HashSet<(int i, int j)> _pairs = new HashSet<(int i, int j)>();

        public const int Theta = 20;

        public CJBDecrypter(string imagePath, string key) : base(imagePath, key) { }

        protected override char DecryptImpl()
        {
            var symbol = 0;

            for (var i = 0; i < 8; i++)
            {
                var sum = 0.0;
                for (var j = 0; j < Theta; j++)
                {
                    var (x, y) = GetRandomPosition();
                    var pixel = UnsafeBitmap[x, y];
                    var predictedBlueBrightness = ComputePredictedBlueBrightness(x, y);
                    sum += pixel.B - predictedBlueBrightness;
                }

                sum /= 15;

                var bit = sum > 0
                        ? 1
                        : 0;

                bit <<= i;

                symbol |= bit;
            }
            var ch = (char)symbol; ;
            return (char)symbol;
        }

        protected override (int i, int j) GetRandomPosition()
        {
            var lengthI = IndexesI.Length - PixelCountCJB;
            var lengthJ = IndexesJ.Length - PixelCountCJB;

            var minBorder = PixelCountCJB;

            var i = Rnd.Next(minBorder, lengthI);
            var j = Rnd.Next(minBorder, lengthJ);

            while (_pairs.Contains((i, j)))
            {
                i = Rnd.Next(minBorder, lengthI);
                j = Rnd.Next(minBorder, lengthJ);
            }

            _pairs.Add((i, j));
            for (var index = 1; index <= PixelCountCJB; index++)
            {
                _pairs.Add((i + index, j));
                _pairs.Add((i - index, j));
                _pairs.Add((i, j + index));
                _pairs.Add((i, j - index));
            }
            return (i, j);
        }

        private double ComputePredictedBlueBrightness(int i, int j)
        {
            var predictedBlueBrightness = (int)UnsafeBitmap[i, j].B;

            var count = i + PixelCountCJB;

            for (var x = i - PixelCountCJB; x <= count; x++)
            {
                predictedBlueBrightness += UnsafeBitmap[x, j].B;
            }

            count = j + PixelCountCJB;

            for (var x = j - PixelCountCJB; x <= count; x++)
            {
                predictedBlueBrightness += UnsafeBitmap[i, x].B;
            }

            predictedBlueBrightness -= 2 * UnsafeBitmap[i, j].B;
            predictedBlueBrightness /= 4 * PixelCountCJB;

            return predictedBlueBrightness;
        }
    }
}
