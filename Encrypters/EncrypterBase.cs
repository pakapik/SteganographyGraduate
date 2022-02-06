using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Encrypters
{
    public abstract class EncrypterBase : IEncrypter
    {
        public abstract Bitmap Encrypt();

        protected readonly UnsafeBitmap _unsafeBitmap;

        protected readonly int _rows;
        protected readonly int _colls;

        protected string _messageToEncrypt;
        protected readonly string _key;

        public EncrypterBase(string imagePath, string messagePath, string key)
        {
            _key = key;
            _unsafeBitmap = new UnsafeBitmap(imagePath);

            _rows = _unsafeBitmap.Width;
            _colls = _unsafeBitmap.Height;
        }

        protected abstract string GetMessageToEcnrypt(string messagePath);

        protected virtual string ReadMessageFromFile(string txtPath)
        {
            var srcMessage = File.ReadAllText(txtPath);

            var dataUtf8 = Encoding.UTF8.GetBytes(srcMessage);
            var dataAscii = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, dataUtf8);
            var messageToEncrypt = Encoding.ASCII.GetString(dataAscii);

            return messageToEncrypt;
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
                    _unsafeBitmap.Dispose();
                }

                _disposed = true;
            }
        }

        ~EncrypterBase() => Dispose(false);

        #endregion
    }
}