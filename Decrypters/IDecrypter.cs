using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Decrypters
{
    public interface IDecrypter : IDisposable
    {
        unsafe IList<byte> Decrypt();
    }
}
