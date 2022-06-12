using System;
using System.Collections.Generic;

namespace SteganographyGraduate.Models.Decrypters
{
    public interface IDecrypter : IDisposable
    {
        unsafe IList<byte> Decrypt();
    }
}
