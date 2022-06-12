using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SteganographyGraduate.Models.Decrypters
{
    [PutInIoC(nameof(DCTDecrypter))]
    public class DCTDecrypter : DecrypterBase, IDCTCrypter
    {
        private readonly int _startX;
        private readonly int _endX;
                
        private readonly int _startY;
        private readonly int _endY;

        private bool initFlag = false;

        public const int BlockSize = 8;
        public const int PValue = 25;

        private readonly Lazy<byte[,]> _bSegments;

        private Point _indexForHidding1 = new Point(6, 3);
        private Point _indexForHidding2 = new Point(3, 6);

        private string _text;
        private int _index;

        private HashSet<int> _segmentIndexForEmded = new HashSet<int>();

        private readonly List<double[,]> _dctList = new List<double[,]>();

        public DCTDecrypter(string imagePath, string key) : base(imagePath, key)
        {
            _startX = Columns % BlockSize;
            _endX = Columns - _startX;

            _startY = Rows % BlockSize;
            _endY = Rows - _startY;
            _bSegments = new Lazy<byte[,]>(() => this.GetChannels(UnsafeBitmap, _endX, _endY, pixel => pixel.B));
        }

        protected override char DecryptImpl()
        {
            if(!initFlag)
            {
                Init();
                initFlag = true;
                _text = GetHiddenText();
            }

            if(_index >= _text.Length)
            {
                return MarkingAvailabilityInformation.EndMetaInfoSymbol;
            }

            var symbol = _text[_index];
            _index++;

            return symbol;
        }

        private void Init()
        {
            var segments = this.SeparateImageOnSegments(_bSegments.Value, _endX, _endY, BlockSize);

            foreach (var segment in segments)
            {
                _dctList.Add(this.DoDCT(segment));
            }
        }

        private string GetHiddenText()
        {
            var symbol = GetHiddenSymbol();
            if (symbol != ((byte)MarkingAvailabilityInformation.StartSymbol))
            {
                return string.Empty;
            }

            var (start, end) = GetTextPositions();

            var txtByte = new byte[end - start];
            for (var i = 0; start < end; i++, start++)
            {
                symbol = GetHiddenSymbol();
                if(start + 1 < end && symbol == (byte)MarkingAvailabilityInformation.EndMetaInfoSymbol)
                {
                    symbol = (byte)'_';
                }

                txtByte[i] = symbol;
            }

            return Encoding.UTF8.GetString(txtByte);
        }

        private (int start, int end) GetTextPositions()
        {
            var textLength = new List<byte>();
            var textLengthInByte = GetHiddenSymbol();

            var end = textLengthInByte + 2;
            var i = 2;
            for (; i < end; i++)
            {
                textLength.Add(GetHiddenSymbol());
                if (i + 1 == end)
                {
                    end += BitOfBytesToInt(textLength);
                    i++;
                    break;
                }
            }

            return (i, end);
        }

        private byte GetHiddenSymbol()
        {
            var symbol = 0;
            var offset = 7;
            for (int j = 0; j < 8; j++)
            {
                var index = this.GetRandomIndexOfSegments(_segmentIndexForEmded, Rnd, _dctList);

                var absValue1 = Math.Abs(_dctList[index][_indexForHidding1.X, _indexForHidding1.Y]);
                var absValue2 = Math.Abs(_dctList[index][_indexForHidding2.X, _indexForHidding2.Y]);

                var bit = absValue1 > absValue2
                        ? 0
                        : 1;

                bit <<= offset;
                offset--;
                symbol |= bit;
            }

            return (byte)symbol;
        }

        private int BitOfBytesToInt(List<byte> bytes)
        {
            var ans = 0;
            for (int i = 0; i < bytes.Count; i++)
            {
                ans |= bytes[i] << (i * 8);
            }

            return ans;
        }
    }
}
