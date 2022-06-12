using NUnit.Framework;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace SteganographyGraduate.Models.Encrypters
{
    public abstract class EncrypterBase : CrypterBase, IEncrypter
    {
        protected string MessageToEncrypt;

        public EncrypterBase(string imagePath, string messagePath, string key) : base(imagePath, key)
        {
            MessageToEncrypt = GetMessageToEcnrypt(messagePath);
        }

        public Bitmap Encrypt()
        {
            var textIndex = 0;
            var textLength = MessageToEncrypt.Length;

            UnsafeBitmap.LockBitmap();

            for (; textIndex < textLength;)
            {
                textIndex = Encrypt(textIndex);
            }

            UnsafeBitmap.UnlockBitmap();

            return UnsafeBitmap.Bitmap;
        }

        public abstract bool CheckCapacity();

        protected abstract int Encrypt(int textIndex);

        protected virtual string GetMessageToEcnrypt(string messagePath)
        {
            var message = ReadMessageFromFile(messagePath);

            return $"{message}" +
                   $"{MarkingAvailabilityInformation.EndMetaInfoSymbol}";
        }

        protected string ReadMessageFromFile(string txtPath)
        {
            var srcMessage = File.ReadAllText(txtPath);

            var dataUtf8 = Encoding.UTF8.GetBytes(srcMessage);
            var messageToEncrypt = Encoding.UTF8.GetString(dataUtf8);

            return messageToEncrypt;
        }   
    }
}