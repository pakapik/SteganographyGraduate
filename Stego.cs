using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public static class StegoHelper
    {
        public static string DefaultKey => "someKey";
        public static int PixelCountToEcnryptLSB => 3;

        public static int GetSeed(string key)
        {
            if(string.IsNullOrEmpty(key))
            {
                key = DefaultKey;
            }

            var seed = 1;

            for (var i = 0; i < key.Length - 1; i++)
            {
                seed *= key[i] * (key[i] - key[i + 1]);
            }

            return seed;
        }
    }
}
