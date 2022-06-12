using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Models.Analyzers
{
    [PutInIoC(nameof(RSAnalyzer))]
    public class RSAnalyzer : AnalyzerBase
    {
        public (RSGroupResult Result, RSGroupResult ResultWithFlipping) ResultNonOverlapping { get; private set; }
        public double PercentMessageLengthNonOverlapping { get; private set; }

        public (RSGroupResult Result, RSGroupResult ResultWithFlipping) ResultOverlapping { get; private set; }
        public double PercentMessageLengthOverlapping { get; private set; }

        private const int DimensionM = 2;
        private const int DimensionN = 2;
        private readonly int[][] _mask = new int[DimensionM][];

        public RSAnalyzer(string imagePath) : base(imagePath)
        {
            for (var i = 0; i < _mask.Length; i++)
            {
                _mask[i] = new int[DimensionM * DimensionN];
            }

            var k = 0;
            for (var i = 0; i < DimensionN; i++)
            {
                for (var j = 0; j < DimensionM; j++)
                {
                    if ((j % 2 == 0 && i % 2 == 0) || (j % 2 == 1 && i % 2 == 1))
                    {
                        _mask[0][k] = 1;
                        _mask[1][k] = 0;
                    }
                    else
                    {
                        _mask[0][k] = 0;
                        _mask[1][k] = 1;
                    }

                    k++;
                }
            }
        }

        public unsafe override bool Analyze()
        {
            var r = Analyze(false, pixel => pixel.R);
            var rr = Analyze(false, pixel => pixel.G);
            var rrr = Analyze(false, pixel => pixel.B);

            ResultNonOverlapping = ((r.Result + rr.Result + rrr.Result) / 3, (r.ResultWithFlipping + rr.ResultWithFlipping + rrr.ResultWithFlipping) / 3);

            r = Analyze(true, pixel => pixel.R);
            rr = Analyze(true, pixel => pixel.G);
            rrr = Analyze(true, pixel => pixel.B);

            ResultOverlapping = ((r.Result + rr.Result + rrr.Result) / 3, (r.ResultWithFlipping + rr.ResultWithFlipping + rrr.ResultWithFlipping) / 3);

            PercentMessageLengthNonOverlapping /= 3;
            PercentMessageLengthOverlapping /= 3;

            return PercentMessageLengthNonOverlapping > 0.1;
        }

        public override string GetResult()
        {
            return $"RS-analyze {_imagePath}, all channel:\n" +
              //  $"Non-overlapping:\n" +
                $"Percetange message length: {PercentMessageLengthNonOverlapping * 100:F2}%\n" +
                $"Regular: {ResultNonOverlapping.Result.CountPositiveRegular}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountPositiveRegular}\n" +
                $"Singular: {ResultNonOverlapping.Result.CountPositiveSingular}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountPositiveSingular}\n" +
                $"Unusable: {ResultNonOverlapping.Result.CountPositiveUnusable}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountPositiveUnusable}\n" +
                $"Negative regular: {ResultNonOverlapping.Result.CountNegativeRegular}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountNegativeRegular}\n" +
                $"Negative singular: {ResultNonOverlapping.Result.CountNegativeSingular}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountNegativeSingular}\n" +
                $"Negative unusable: {ResultNonOverlapping.Result.CountNegativeUnusable}. With flipping: {ResultNonOverlapping.ResultWithFlipping.CountNegativeUnusable}\n";// +
                //$"\n" +
                //$"Overlapping:\n" +
                //$"Percetange message length: {(PercentMessageLengthOverlapping * 100):F2}%\n" +
                //$"Regular: {ResultOverlapping.Result.CountPositiveRegular}. With flipping: {ResultOverlapping.ResultWithFlipping.CountPositiveRegular}\n" +
                //$"Singular: {ResultOverlapping.Result.CountPositiveSingular}. With flipping: {ResultOverlapping.ResultWithFlipping.CountPositiveSingular}\n" +
                //$"Unusable: {ResultOverlapping.Result.CountPositiveUnusable}. With flipping: {ResultOverlapping.ResultWithFlipping.CountPositiveUnusable}\n" +
                //$"Negative regular: {ResultOverlapping.Result.CountNegativeRegular}. With flipping: {ResultOverlapping.ResultWithFlipping.CountNegativeRegular}\n" +
                //$"Negative singular: {ResultOverlapping.Result.CountNegativeSingular}. With flipping: {ResultOverlapping.ResultWithFlipping.CountNegativeSingular}\n" +
                //$"Negative unusable: {ResultOverlapping.Result.CountNegativeUnusable}. With flipping: {ResultOverlapping.ResultWithFlipping.CountNegativeUnusable}\n";
        }

        public (RSGroupResult Result, RSGroupResult ResultWithFlipping) Analyze(bool overlap, Func<Pixel, byte> getChannel)
        {
            var rsResult = GetRSGroupResult(overlap, false, getChannel);
            var rsResultFlip = GetRSGroupResult(overlap, true, getChannel);

            var x = GetX(rsResult, rsResultFlip, overlap);

            var flippedEstimatedPercent = (2 * (x - 1)).EqualTo(0, 1e-5)
                                        ? 0 
                                        : Math.Abs(x / (2 * (x - 1)));

            var messageLength = (x - 0.5).EqualTo(0, 1e-5)
                              ? 0 
                              : Math.Abs(x / (x - 0.5));

            return (rsResult, rsResultFlip);
        }

        private unsafe RSGroupResult GetRSGroupResult(bool overlap, bool isNeedledFlip, Func<Pixel, byte> getChannel)
        {
            var rsResult = new RSGroupResult();
            var startX = 0;
            var startY = 0;
            var block = new Pixel[DimensionM * DimensionN]; // not recreate
            UnsafeBitmap.LockBitmap();
            while (startX < Rows && startY < Columns)
            {
                for (var m = 0; m < DimensionM; m++)
                {
                    FillBlock(block, startX, startY);

                    if (isNeedledFlip)
                    {
                        block = FlipBlockIfIsNeddled(block);
                    }

                    rsResult.AllVariations = GetVariation(block, getChannel);

                    block = FlipBlock(block, _mask[m]);
                    rsResult.PositiveVariations = GetVariation(block, getChannel);

                    block = FlipBlock(block, _mask[m]);
                    _mask[m] = InvertMask(_mask[m]);
                    rsResult.NegativeVariations = GetNegativeVariation(block, _mask[m], getChannel);
                    _mask[m] = InvertMask(_mask[m]);

                    rsResult.UpdateGroups();
                }

                GetNextPosition(overlap, ref startX, ref startY);

                if (startY >= Columns - 1)
                {
                    break;
                }
            }
            UnsafeBitmap.UnlockBitmap();
            return rsResult;
        }

        private void GetNextPosition(bool overlap, ref int startX, ref int startY)
        {
            if (overlap)
            {
                startX += 1;
            }
            else
            {
                startX += DimensionM;
            }

            if (startX >= Rows - 1)
            {
                startX = 0;
                if (overlap)
                {
                    startY += 1;
                }
                else
                {
                    startY += DimensionN;
                }
            }
        }

        private unsafe void FillBlock(Pixel[] block, int startX, int startY)
        {
            var k = 0;
            for (var i = 0; i < DimensionN; i++)
            {
                for (var j = 0; j < DimensionM; j++)
                {
                    block[k] = UnsafeBitmap[startY + i, startX + j];
                    k++;
                }
            }
        }

        private int GetVariation(Pixel[] block, Func<Pixel, byte> getChannel)
        {
            var colorSum = 0;
            for (var i = 0; i < block.Length; i += 4)
            {
                colorSum += Math.Abs(getChannel(block[i]) - getChannel(block[1 + i]));
                colorSum += Math.Abs(getChannel(block[3 + i]) - getChannel(block[2 + i]));
                colorSum += Math.Abs(getChannel(block[1 + i]) - getChannel(block[3 + i]));
                colorSum += Math.Abs(getChannel(block[2 + i]) - getChannel(block[i]));
            }

            return colorSum;
        }

        private Pixel[] FlipBlock(Pixel[] block, int[] mask)
        {
            for (var i = 0; i < block.Length; i++)
            {
                int red = block[i].R;
                int green = block[i].G;
                int blue = block[i].B;

                if (mask[i] == 1)
                {                
                    red = NegateLSB(red);
                    green = NegateLSB(green);
                    blue = NegateLSB(blue);                
                }
                else if (mask[i] == -1)
                {
                    red = InvertLSB(red);
                    green = InvertLSB(green);
                    blue = InvertLSB(blue);
                }

                var newPixel = (0xff << 24) | ((red & 0xff) << 16) | ((green & 0xff) << 8) | (blue & 0xff);

                block[i] = Color.FromArgb(newPixel);
            }

            return block;
        }

        private int NegateLSB(int colorChannel)
        {
            var temp = colorChannel & 0xfe;
            return temp == colorChannel 
                 ? colorChannel 
                 | 0x1 : temp;
        }

        private int InvertLSB(int colorChannel)
        {
            if (colorChannel == 255)
            {
                return 256;
            }

            if (colorChannel == 256)
            {
                return 255;
            }

            return NegateLSB(colorChannel + 1) - 1;
        }

        private int[] InvertMask(int[] mask)
        {
            for (var i = 0; i < mask.Length; i++)
            {
                mask[i] = mask[i] * -1;
            }

            return mask;
        }

        private int GetNegativeVariation(Pixel[] block, int[] mask, Func<Pixel, byte> getColor)
        {       
            int InvertIfNegative(int maskValue, int value) => maskValue == -1 ? InvertLSB(value) : value;

            int GetDifficultSum(int index1, int index2)
            {
                var value1 = InvertIfNegative(mask[index1], getColor(block[index1]));
                var value2 = InvertIfNegative(mask[index2], getColor(block[index2]));

                return Math.Abs(value1 - value2);
            }
           
            var colorChannel = 0;
            for (var i = 0; i < block.Length; i += 4)
            {
                var m = i;
                var n = 1 + i;
                colorChannel += GetDifficultSum(m, n);

                m = n;
                n = 3 + i;
                colorChannel += GetDifficultSum(m, n);

                m = n;
                n = 2 + i;
                colorChannel += GetDifficultSum(m, n);

                m = n;
                n = i;
                colorChannel += GetDifficultSum(m, n);
            }

            return colorChannel;
        }
       
        private Pixel[] FlipBlockIfIsNeddled(Pixel[] block)
        {
            var mask = new int[DimensionM * DimensionN];

            for (var i = 0; i < mask.Length; i++)
            {
                mask[i] = 1;
            }

            return FlipBlock(block, mask);
        }

        private double GetX(RSGroupResult rsResult, RSGroupResult rsResultFlip, bool overlap)
        {
            var r = (double)rsResult.CountPositiveRegular;
            var rm = (double)rsResult.CountNegativeRegular;
            var r1 = (double)rsResultFlip.CountPositiveRegular;
            var rm1 = (double)rsResultFlip.CountNegativeRegular;
            var s = (double)rsResult.CountPositiveSingular;
            var sm = (double)rsResult.CountNegativeSingular;
            var s1 = (double)rsResultFlip.CountPositiveSingular;
            var sm1 = (double)rsResultFlip.CountNegativeSingular;

            var x = 0.0;

            var dZero = r - s; // d0 = Rm(p/2) - Sm(p/2)
            var dMinusZero = rm - sm; // d-0 = R-m(p/2) - S-m(p/2)
            var dOne = r1 - s1; // d1 = Rm(1-p/2) - Sm(1-p/2)
            var dMinusOne = rm1 - sm1; // d-1 = R-m(1-p/2) - S-m(1-p/2)

            // get x as the root of the equation 
            // 2(d1 + d0)x^2 + (d-0 - d-1 - d1 - 3d0)x + d0 - d-0 = 0
            // x = (-b +or- sqrt(b^2-4ac))/2a
            // where ax^2 + bx + c = 0 and this is the form of the equation

            var a = 2 * (dOne + dZero);
            var b = dMinusZero - dMinusOne - dOne - 3 * dZero;
            var c = dZero - dMinusZero;

            if (a == 0)
            {
                x = c / b;
            }
              
            var discriminant = Math.Pow(b, 2) - 4 * a * c;

            if (discriminant >= 0)
            {
                var positiveRoot = (-b + Math.Sqrt(discriminant)) / (2 * a);
                var negativeRoot = (-b - Math.Sqrt(discriminant)) / (2 * a);

                x = Math.Abs(positiveRoot) <= Math.Abs(negativeRoot) 
                    ? positiveRoot
                    : negativeRoot;
            }
            else
            {
                var cr = (rm - r) / (r1 - r + rm - rm1);
                var cs = (sm - s) / (s1 - s + sm - sm1);

                x = (cr + cs) / 2;
            }

            if (x == 0)
            {
                var rr = (rm1 - r1 + r - rm + (rm - r) / x) / (x - 1);
                var ss = (sm1 - s1 + s - sm + (sm - s) / x) / (x - 1);
                if (ss > 0 | rr < 0)
                {
                    var cr = (rm - r) / (r1 - r + rm - rm1);
                    var cs = (sm - s) / (s1 - s + sm - sm1);
                    x = (cr + cs) / 2;
                }
            }

            var p = x / (x - 0.5);

            if(overlap)
            {
                PercentMessageLengthOverlapping += p;
            }
            else
            {
                PercentMessageLengthNonOverlapping += p;
            }
           
            return x;
        }    
    }
}
