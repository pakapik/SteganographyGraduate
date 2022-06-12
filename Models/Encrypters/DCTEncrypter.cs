using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SteganographyGraduate.Models.Encrypters
{
    [PutInIoC(nameof(DCTEncrypter))]
    public class DCTEncrypter : EncrypterBase, IDCTCrypter
    {
        // TODO: по-хорошему, надо было ещё один базовый класс выделить или сущность-контейнер
        // вместо расширений для IDCTCrypter и прокидываний параметров.

        private readonly int _startX;
        private readonly int _endX;

        private readonly int _startY;
        private readonly int _endY;
    
        private Point _indexForHidding1 = new Point(6, 3);
        private Point _indexForHidding2 = new Point(3, 6);

        private HashSet<int> _segmentIndexForEmded = new HashSet<int>();

        private byte[] _masks = new byte[]
        {
            0b1000_0000, // 128
            0b0100_0000, // 64
            0b0010_0000, // 32
            0b0001_0000, // 16
            0b1000_1000, // 8
            0b1000_0100, // 4
            0b1000_0010, // 2
            0b1000_0001  // 1
        };

        public const int BlockSize = 8;
        public const int PValue = 25;

        public DCTEncrypter(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key)
        {
            _startX = Columns % BlockSize; 
            _endX = Columns - _startX;

            _startY = Rows % BlockSize;
            _endY = Rows - _startY;
        }

        public override bool CheckCapacity() => _endY * _endX / (BlockSize * BlockSize) >= MessageToEncrypt.Length * BlockSize;

        protected override int Encrypt(int textIndex)
        {
            var channelValues = this.GetChannels(UnsafeBitmap, _endX, _endY, pixel => pixel.B);
            var dct = this.SeparateImageOnSegments(channelValues, _endX, _endY, BlockSize)
                .Select(this.DoDCT)
                .ToArray();

            EmbedDataIntoDct(dct);
            var idct = dct.Select(DoIDCT).ToArray();
            var newSegments = JoinSegments(idct);

            NormalizeChannel(newSegments);
            EmdedToImage(newSegments);

            return MessageToEncrypt.Length;
        }

        private static void NormalizeChannel(double[,] idct)
        {
            var min = double.MaxValue;
            var max = double.MinValue;
            var columns = idct.GetLength(0);
            var rows = idct.GetLength(1);
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var value = idct[i, j];
                    if (value > max)
                    {
                        max = value;
                    }
                    if (value < min)
                    {
                        min = value;
                    }
                }
            }

            min = Math.Abs(min);
            max += min;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    idct[i, j] = 255 * (idct[i, j] + min) / max;
                }
            }
        }

        private void EmdedToImage(double[,] channelValues)
        {
            for (var x = 0; x < _endX; x++)
            {
                for (var y = 0; y < _endY; y++)
                {
                    var pixel = UnsafeBitmap[x, y];
                    pixel.B = (byte)Math.Round(channelValues[x, y]);
                    UnsafeBitmap[x, y] = pixel;
                }
            }
        }
    
        private void EmbedDataIntoDct(double[][,] dct)
        {
            var dataToEncrypt = GetDataToEcnrypt();

            for (int i = 0; i < dataToEncrypt.Length; i++)
            {
                var dataByte = dataToEncrypt[i];
                var offset = 7;
                for (var j = 0; j < 8; j++)
                {
                    EmbedBitIntoDct(dct, (dataByte & _masks[j]) >> offset);
                    offset--;
                }
            }
        }

        private void EmbedBitIntoDct(double[][,] dct, int bit)
        {
            var index = this.GetRandomIndexOfSegments(_segmentIndexForEmded, Rnd, dct);
            var value1 = dct[index][_indexForHidding1.X, _indexForHidding1.Y];
            var value2 = dct[index][_indexForHidding2.X, _indexForHidding2.Y];
            var absValue1 = Math.Abs(value1);
            var absValue2 = Math.Abs(value2);

            var z1 = 1;
            var z2 = 1;
            if (value1 < 0)
            {
                z1 = -1;
            }
            if (value2 < 0)
            {
                z2 = -1;
            }

            if (bit == 1)
            {
                if (absValue1 - absValue2 >= -PValue)
                {
                    absValue2 = PValue + absValue1 + 1;
                }
            }
            else
            {
                if (absValue1 - absValue2 <= PValue)
                {
                    absValue1 = PValue + absValue2 + 1;
                }
            }

            dct[index][_indexForHidding1.X, _indexForHidding1.Y] = z1 * absValue1;
            dct[index][_indexForHidding2.X, _indexForHidding2.Y] = z2 * absValue2;
        }

        private byte[] GetDataToEcnrypt()
        {
            var messageToEncrypt = Encoding.UTF8.GetBytes(MessageToEncrypt);

            var lengthInBytes = GetByteArrayWithValue(MessageToEncrypt.Length);
            var txtByte = new byte[2 + lengthInBytes.Length + messageToEncrypt.Length];
            txtByte[0] = (byte)MarkingAvailabilityInformation.StartSymbol;
            txtByte[1] = CountSignificantBytes(MessageToEncrypt.Length);

            var index = 0;
            for (int i = 0; i < lengthInBytes.Length; i++)
            {
                txtByte[i + 2] = lengthInBytes[index];
                index++;
            }

            index = 0;
            for (int i = 0; i < messageToEncrypt.Length; i++)
            {
                txtByte[i + 2 + lengthInBytes.Length] = messageToEncrypt[index];
                index++;
            }

            return txtByte;
        }

        private byte[] GetByteArrayWithValue(int value)
        {
            var countSignificatBytes = CountSignificantBytes(value);

            if (countSignificatBytes == 1)
            {
                return new byte[] { (byte)value };
            }
            else if (countSignificatBytes == 2)
            {
                return BitConverter.GetBytes((short)value);
            }

            return BitConverter.GetBytes(value);
        }

        private byte CountSignificantBytes(int positiveInteger)
        {
            if (positiveInteger < 0)
            {
                throw new ArgumentException($"{positiveInteger} not positive");
            }

            if (positiveInteger > 0 && positiveInteger < 256)
            {
                return 1;
            }
            else if (positiveInteger < 65536)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }

        private double[,] DoIDCT(double[,] dct)
        {
            var length = dct.GetLength(0);
            var idct = new double[length, length];
            var doubleLength = 2 * length;
            var sqrtDoubleLength = Math.Sqrt(doubleLength);
            for (var i = 0; i < length; i++)
            {
                var ii = 2 * i + 1;
                for (int j = 0; j < length; j++)
                {
                    var temp = 0.0;
                    var piJ = Math.PI * (2 * j + 1);
                    for (var x = 0; x < length; x++)
                    {
                        var coefX = this.GetCoefficient(x);
                        var xCos = Math.Cos(Math.PI * x * ii / doubleLength);
                        for (var y = 0; y < length; y++)
                        {
                            temp += coefX * this.GetCoefficient(y)
                                * dct[x, y]
                                * xCos
                                * Math.Cos(piJ * y / doubleLength);
                        }
                    }

                    idct[i, j] = temp / sqrtDoubleLength;
                }
            }

            return idct;
        }

        private double[,] JoinSegments(double[][,] idct)
        {
            var segments = new double[_endX, _endY];

            var numSW = _endX / BlockSize;
            var numSH = _endY / BlockSize; 
            var k = 0;
            for (var i = 0; i < numSW; i++)
            {
                var firstWPoint = i * BlockSize; 
                var lastWPoint = firstWPoint + BlockSize - 1; 
                for (var j = 0; j < numSH; j++)
                {
                    var firstHPoint = j * BlockSize;
                    var lastHPoint = firstHPoint + BlockSize - 1;
                    Insert(segments, idct[k], firstWPoint, lastWPoint, firstHPoint, lastHPoint);
                    k++;
                }
            }

            return segments;
        }
    
        private static void Insert(double[,] arr, double[,] temp, int firstWPoint, int lastWPoint, int firstHPoint, int lastHPoint)
        {
            for (int i = firstWPoint, u = 0; i < lastWPoint + 1; i++, u++)
            {
                for (int j = firstHPoint, v = 0; j < lastHPoint + 1; j++, v++)
                    arr[i, j] = temp[u, v];
            }
        }
    }
}
