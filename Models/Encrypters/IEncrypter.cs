using System;
using System.Drawing;

namespace SteganographyGraduate.Models.Encrypters
{
    public interface IEncrypter : IDisposable
    {
        unsafe Bitmap Encrypt();

        bool CheckCapacity();
    }
}
