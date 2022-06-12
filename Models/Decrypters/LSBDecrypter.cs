using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Models.Decrypters
{
    [PutInIoC(nameof(LSBDecrypter))]
    public class LSBDecrypter : DecrypterBase
    {
        public LSBDecrypter(string imagePath, string key) : base(imagePath, key) { }

        protected override unsafe char DecryptImpl()
        {
            var (i, j) = GetRandomPosition();
            var p1 = UnsafeBitmap[i, j];
           
            var (n, m) = GetRandomPosition();
            var p2 = UnsafeBitmap[n, m];
           
            var (k, t) = GetRandomPosition();
            var p3 = UnsafeBitmap[k, t];
 
            return DecryptPixels(p1, p2, p3);        
        }

        private char DecryptPixels(Pixel p1, Pixel p2, Pixel p3)
        {
            var bitR = (p1.R & 1) << 7;
            var bitG = (p1.G & 1) << 6;
            var bitB = (p1.B & 1) << 5;
            var result = bitR | bitG | bitB;

            bitR = (p2.R & 1) << 4;
            bitG = (p2.G & 1) << 3;
            bitB = (p2.B & 1) << 2;
            result |= bitR | bitG | bitB;

            bitR = (p3.R & 1) << 1;
            bitG = p3.G & 1;
            result |= bitR | bitG;

            return (char)result;
        }
    }
}
