using System;
using System.IO;
using System.Reflection;

namespace CryptoSSL.Tests.Helpers
{
    public static class TestHelper
    {
        public static string GetTestFullDataPath(string testDataPath)
        {
            var testAssembly = Assembly.GetCallingAssembly();
            return Path.Combine(testAssembly.GetAssemblyDirectory(), testDataPath);
        }

        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(GetAssemblyPath(assembly));
        }

        public static string GetAssemblyPath(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            var codeBase = assembly.CodeBase;
            if (codeBase == null)
            {
                throw new ArgumentException("Specified assembly has no physical code base.");
            }

            var uri = new Uri(codeBase);
            if (!uri.IsFile)
            {
                throw new ArgumentException("Specified assembly placed not on local disk.");
            }

            return uri.AbsolutePath;
        }
    }
}
