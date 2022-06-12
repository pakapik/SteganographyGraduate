using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteganographyGraduate;

namespace SteganographyGraduate.Models.Decrypters
{
    [PutInIoC(nameof(RLBDecrypter))]
    public class RLBDecrypter : DecrypterBase
    {
        public RLBDecrypter(string imagePath, string key) : base(imagePath, key) { }
    
        protected override unsafe char DecryptImpl()
        {
            var (i, j) = GetRandomPosition();

            return DecryptPixel(UnsafeBitmap[i, j]);
        }

        private char DecryptPixel(Pixel pixel)
        {
            var b = pixel.B & 7;
            var g = (pixel.G & 3) << 3;
            var r = (pixel.R & 7) << 5;

            var result = r | g | b;

            return (char)result;
        }
    }
}
