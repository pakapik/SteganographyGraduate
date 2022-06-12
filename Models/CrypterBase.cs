using System;
using System.Collections.Generic;

namespace SteganographyGraduate.Models
{
    public abstract class CrypterBase
    {
        protected enum BitState : byte
        {
            IsEmpty,
            IsFull 
        }

        protected readonly UnsafeBitmap UnsafeBitmap;

        protected readonly int Rows;
        protected readonly int Columns;

        protected readonly string Key;

        protected readonly Random Rnd;

        protected readonly BitState[] IndexesI;
        protected readonly BitState[] IndexesJ;

        protected readonly HashSet<(int i, int j)> _coordinates = new HashSet<(int i, int j)>();

        protected const string DefaultKey = "someKey";

        protected const int PixelCountLSB = 3;
        protected const int PixelCountCJB = 3;

        public CrypterBase(string imagePath, string key)
        {
            UnsafeBitmap = new UnsafeBitmap(imagePath);

            Rows = UnsafeBitmap.Height;
            Columns = UnsafeBitmap.Width;

            Key = string.IsNullOrEmpty(key) 
                ? DefaultKey 
                : key;

            Rnd = new Random(GetSeed(Key));

            IndexesI = new BitState[Columns];
            IndexesJ = new BitState[Rows];
        }

        protected virtual (int i, int j) GetRandomPosition()
        {
            var lengthI = IndexesI.Length;
            var lengthJ = IndexesJ.Length;
            var i = Rnd.Next(0, lengthI);
            var j = Rnd.Next(0, lengthJ);

            while (_coordinates.Contains((i, j)))
            {
                i = Rnd.Next(0, lengthI);
                j = Rnd.Next(0, lengthJ);
            }

            _coordinates.Add((i, j));

            return (i, j);
        }
        
        private int GetSeed(string key)
        {
            var seed = 1;

            for (var i = 0; i < key.Length - 1; i++)
            {
                seed *= key[i] * (key[i] - key[i + 1]);
            }

            return seed;
        }

        #region Dispose

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    UnsafeBitmap.Dispose();
                }

                _disposed = true;
            }
        }

        ~CrypterBase() => Dispose(false);

        #endregion

    }
}
