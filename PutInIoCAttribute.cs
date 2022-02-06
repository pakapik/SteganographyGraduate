using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public class PutInIoCAttribute : Attribute
    {
        public string Name { get; }

        public PutInIoCAttribute(string name) => Name = name;
    }
}
