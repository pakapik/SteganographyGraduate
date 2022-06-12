using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Models
{
    public static class IDCTCrypterExtensions
    {
        private static readonly double _coefficient = 1.0 / Math.Sqrt(2);

        public static double GetCoefficient(this IDCTCrypter crypter, int arg) => arg == 0 ? _coefficient : 1;

        public static byte[,] GetChannels(this IDCTCrypter crypter, UnsafeBitmap bmp, int endX, int endY, Func<Pixel, byte> getChannel)
        {
            var channelValues = new byte[endX, endY];
            for (var x = 0; x < endX; x++)
            {
                for (var y = 0; y < endY; y++)
                {
                    channelValues[x, y] = getChannel(bmp[x, y]);
                }
            }

            return channelValues;
        }

        public static List<byte[,]> SeparateImageOnSegments(this IDCTCrypter crypter, byte[,] bBytes, int endX, int endY, int blockSize)
        {
            var segments = new List<byte[,]>();
            int numSW = endX / blockSize;
            int numSH = endY / blockSize;
            for (var i = 0; i < numSW; i++)
            {
                int firstWPoint = i * blockSize;
                int lastWPoint = firstWPoint + blockSize - 1;
                for (var j = 0; j < numSH; j++)
                {
                    var firstHPoint = j * blockSize;
                    var lastHPoint = firstHPoint + blockSize - 1;
                    segments.Add(GetSegment(bBytes, firstWPoint, lastWPoint, firstHPoint, lastHPoint));
                }
            }

            return segments;
        }

        private static byte[,] GetSegment(byte[,] segments, int a, int b, int c, int d)
        {
            var segment = new byte[b - a + 1, d - c + 1];
            for (int i = a, x = 0; i <= b; i++, x++)
            {
                for (int j = c, y = 0; j <= d; j++, y++)
                {
                    segment[x, y] = segments[i, j];
                }
            }

            return segment;
        }

        public static double[,] DoDCT(this IDCTCrypter crypter, byte[,] channelValues)
        {
            var length = channelValues.GetLength(0);
            var dct = new double[length, length];
            var doubleLength = 2 * length;
            var sqrtDoubleLength = Math.Sqrt(doubleLength);
            for (var i = 0; i < length; i++)
            {
                var piI = Math.PI * i;
                for (var j = 0; j < length; j++)
                {
                    var temp = 0.0;
                    var piJ = Math.PI * j;
                    for (var x = 0; x < length; x++)
                    {
                        var xCos = Math.Cos(piI * (2 * x + 1) / doubleLength);
                        for (var y = 0; y < length; y++)
                        {
                            temp += channelValues[x, y]
                               * xCos
                               * Math.Cos(piJ * (2 * y + 1) / doubleLength);
                        }
                    }

                    dct[i, j] = crypter.GetCoefficient(j) * crypter.GetCoefficient(i) * temp / sqrtDoubleLength;
                }
            }

            return dct;
        }

        public static int GetRandomIndexOfSegments(this IDCTCrypter crypter, HashSet<int> indexes, Random rnd, IList<double[,]> segments)
        {
            var index = rnd.Next(0, segments.Count);

            while (indexes.Contains(index))
            {
                index = rnd.Next(0, segments.Count);
            }

            _ = indexes.Add(index);

            return index;
        }
    }

}
