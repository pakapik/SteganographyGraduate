using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Decrypters
{
    public abstract class DecrypterRandomBase : DecrypterBase
    {
        protected readonly Random _rnd;
        protected readonly int[] _indexesI;
        protected readonly int[] _indexesJ;

        protected DecrypterRandomBase(string imagePath, string key) : base(imagePath, key)
        {
            _rnd = new Random(StegoHelper.GetSeed(key));

            _indexesI = new int[_rows];
            _indexesJ = new int[_colls];
        }


        protected (int i, int j) GetStartPosition()
        {
            var i = _rnd.Next(0, _rows);
            _indexesI[i] = 1;
            var j = _rnd.Next(0, _colls);
            _indexesJ[j] = 1;

            return (i, j);
        }

        protected int FindFreePosition(int[] array)
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
