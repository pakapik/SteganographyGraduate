using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Encrypters
{
    public abstract class EncrypterRandomBase : EncrypterBase
    {
        protected readonly Random _rnd;

        protected readonly int[] _indexesI;
        protected readonly int[] _indexesJ;

        public EncrypterRandomBase(string imagePath, string messagePath, string key) : base(imagePath, messagePath, key)
        {
            _rnd = new Random(StegoHelper.GetSeed(key));

            _indexesI = new int[_rows];
            _indexesJ = new int[_colls];
        }

        protected virtual (int i, int j) GetStartPosition()
        {
            var i = _rnd.Next(0, _rows);
            _indexesI[i] = 1;
            var j = _rnd.Next(0, _colls);
            _indexesJ[j] = 1;

            return (i, j);
        }

        protected virtual int FindFreePosition(int[] array)
        {
            var length = array.Length;

            // -1, т.к. первый вызов уже был для проверки наличия инфы
            var tryes = length - 1;

            var i = _rnd.Next(0, length);

            while (tryes != 0 && array[i] > 0)
            {
                i = _rnd.Next(0, length);
                tryes--;
            }

            array[i] = 1;

            return i;
        }
    }
}
