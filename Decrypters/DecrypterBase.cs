using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Decrypters
{
    public abstract class DecrypterBase : IDecrypter
    {
        public abstract IList<byte> Decrypt();

        protected readonly UnsafeBitmap _unsafeBitmap;

        protected readonly int _rows;
        protected readonly int _colls;

        public DecrypterBase(string imagePath, string key)
        {
            _unsafeBitmap = new UnsafeBitmap(imagePath);
          
            _rows = _unsafeBitmap.Width;
            _colls = _unsafeBitmap.Height;
        }

        protected abstract (int metaInfoLength, int infoLength) GetEncryptInfoLength();

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
                    _unsafeBitmap.Dispose();
                }

                _disposed = true;
            }
        }

        ~DecrypterBase() => Dispose(false);
        #endregion
    }
}
