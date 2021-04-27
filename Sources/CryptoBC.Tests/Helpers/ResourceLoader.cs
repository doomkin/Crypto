using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace CryptoBC.Tests.Helpers
{
    public class ResourceLoader
    {
        private readonly Assembly _assembly;
        private readonly string _prefix;

        public ResourceLoader(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _assembly = assembly;
            _prefix = assembly.GetName().Name;

        }

        public byte[] ReadAllBytes(string path)
        {
            using (var stream = _assembly.GetManifestResourceStream(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", _prefix, path)))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
