using Extreme.Mathematics;
using Extreme.Mathematics.LinearAlgebra;
using Extreme.Statistics.Tests;

using System;
using System.IO;
using System.Windows.Forms;

namespace SteganographyGraduate.Models.Analyzers
{
    [PutInIoC(nameof(ChiSquareAnalyzer))]
    public class ChiSquareAnalyzer : AnalyzerBase
    {
        private const int ColorCount = 256;
        private const int HalfColorCount = ColorCount / 2;
        private const double KValue = (HalfColorCount - 1.0) / 2.0;

        #region saving
        // Этого здесь, вообще говоря, быть не должно.
        // Чисто, чтоб пикчу сейвить.
        private string _path;
        #endregion

        public ChiSquareAnalyzer(string imagePath) : base(imagePath) { }

        public override string GetResult() => $"Гистограммный анализ {_imagePath} произведен. Результат сохранен: {_path}";

        public unsafe override bool Analyze()
        {
            UnsafeBitmap.LockBitmap();

            for (var y = 0; y < Rows; y++)
            {
                var rColors = new byte[ColorCount];
                var gColors = new byte[ColorCount];
                var bColors = new byte[ColorCount];

                for (var x = 0; x < Columns; x++)
                {
                    var pixel = UnsafeBitmap[x, y];
                    rColors[pixel.R]++;
                    gColors[pixel.G]++;
                    bColors[pixel.B]++;
                }

                var frequenciesR = GetFrequencies(rColors);
                var frequenciesG = GetFrequencies(gColors);
                var frequenciesB = GetFrequencies(bColors);

                var vectorR = GetVector(frequenciesR);
                var vectorG = GetVector(frequenciesG);
                var vectorB = GetVector(frequenciesB);

                var chiSquareR = new ChiSquareGoodnessOfFitTest(vectorR.actual, vectorR.expected);
                var chiSquareG = new ChiSquareGoodnessOfFitTest(vectorG.actual, vectorG.expected);
                var chiSquareB = new ChiSquareGoodnessOfFitTest(vectorB.actual, vectorB.expected);

                ChangeColorInRow(y, chiSquareR, chiSquareG, chiSquareB);
            }

            #region saving
            // Этого здесь, вообще говоря, быть не должно.
            // Чисто, чтоб пикчу сейвить.

            UnsafeBitmap.UnlockBitmap();
            var path = Path.GetFileNameWithoutExtension(_imagePath);
            _path = $@"{AppDomain.CurrentDomain.BaseDirectory}_{path}_{DateTime.Now:yyyyMMdd_hhmmss}.png";
            UnsafeBitmap.Bitmap.Save(_path);
            #endregion
            return true;
        }

        private unsafe void ChangeColorInRow(int y, 
            ChiSquareGoodnessOfFitTest chiSquareR,
            ChiSquareGoodnessOfFitTest chiSquareG, 
            ChiSquareGoodnessOfFitTest chiSquareB)
        {
            var p = (chiSquareR.PValue + chiSquareG.PValue + chiSquareB.PValue) / 3;

            if (p > 0.5)
            {
                ChangeRowColor(y, (channel, pixel) =>
                {
                    pixel.R = channel;
                    return pixel;
                });
            }
            else
            {
                ChangeRowColor(y, (channel, pixel) =>
                {
                    pixel.G = channel;
                    return pixel;
                });
            }
        }

        private (Vector<double> actual, Vector<double> expected) GetVector((double[] actual, double[] expected) frequencies)
        {
            var actualVector = Vector.Create(frequencies.actual);
            var expectedVector = Vector.Create(frequencies.expected);

            return (actualVector, expectedVector);
        }

        private void ChangeRowColor(int rowIndex, Func<byte, Pixel, Pixel> func)
        {
            var y = rowIndex;
            for (var x = 0; x < Columns; x++)
            {
                var pixel = UnsafeBitmap[x, y];
                pixel = func(255, pixel);
                UnsafeBitmap[x, y] = pixel;
            }
        }

        private (double[] actual, double [] expected) GetFrequencies(byte[] colorInRows)
        {
            var actualFrequency = new double[HalfColorCount];
            var expectedFrequency = new double[HalfColorCount];
            for (var i = 0; i < HalfColorCount; i++)
            {
                var k = 2 * i;
                actualFrequency[i] = colorInRows[k];
                expectedFrequency[i] = (colorInRows[k] + colorInRows[k + 1]) / 2;
            }

            return (actualFrequency, expectedFrequency);
        }
    }
}
