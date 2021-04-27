using System;
using System.IO;
using Crypto.Digest;
using CryptoSSL.Configurator;
using CryptoSSL.Helpers;

namespace CryptoSSL.Digest
{
    internal abstract class CryptoDigestBase : ICryptoDigest
    {
        private static readonly ExternalCommand ExternalCommand = new ExternalCommand();
        private static readonly Converter Converter = new Converter();
        private readonly string _digestAlgorithmCode;
        private readonly OpenSslConfigurator _configurator;

        protected CryptoDigestBase(string digestAlgorithmCode, OpenSslConfigurator configurator)
        {
            if (string.IsNullOrEmpty(digestAlgorithmCode))
            {
                throw new ArgumentNullException("digestAlgorithmCode");
            }

            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            _digestAlgorithmCode = digestAlgorithmCode;
            _configurator = configurator;
        }

        #region Implementation of ICryptoDigest

        public byte[] Digest(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string stdout;
            // Сохраняем байтовый массив данных во временный файл со случайными именем
            var fileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());
            try
            {
                File.WriteAllBytes(fileName, data);

                // Вызываем openssl и перехватываем stdout
                stdout = ExternalCommand.Call(_configurator.OpenSslCommand, "dgst -" + _digestAlgorithmCode + " " + fileName);
            }
            finally
            {
                // Удаляем временный файл
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }

            // Извлекаем строку с шеснадцатиричным хешем и преобразуем его в байтовый массив
            var hashHexStr = stdout.Substring(stdout.IndexOf(")= ", StringComparison.InvariantCulture) + 3).TrimEnd();
            var hash = Converter.ToByteArray(hashHexStr);

            return hash;
        }

        #endregion
    }
}
