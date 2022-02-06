using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public class IoCContainer
    {
        public static IoCContainer Instance => _lazy.Value;

        private static readonly Lazy<IoCContainer> _lazy =  new Lazy<IoCContainer>(() => new IoCContainer());

        private readonly Dictionary<string, string> _classes = new Dictionary<string, string>();

        private readonly Assembly _assembly;

        private IoCContainer()
        {
            _assembly = Assembly.GetExecutingAssembly();

            _classes = _assembly.GetTypes().Where(x => x.GetCustomAttribute<PutInIoCAttribute>() != null)
                                           .ToDictionary(x => x.Name, y => y.FullName);
        }

        public T Create<T>(string nameObject, object[] @params)
        {
            return (T)_assembly.CreateInstance(_classes[nameObject],
                                               false,
                                               BindingFlags.Public | BindingFlags.Instance,
                                               null,
                                               @params,
                                               null,
                                               null);
        }
    }
}
