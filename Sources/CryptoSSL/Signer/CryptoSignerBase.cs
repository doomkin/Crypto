using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Crypto.Signer;
using CryptoSSL.Configurator;
using CryptoSSL.Helpers;


namespace CryptoSSL.Signer
{
    internal abstract class CryptoSignerBase : ICryptoSigner
    {
        private static readonly ExternalCommand ExternalCommand = new ExternalCommand();
        private readonly string _digestAlgorithmCode;
        private readonly OpenSslConfigurator _configurator ;

        protected CryptoSignerBase(string digestAlgorithmCode, OpenSslConfigurator configurator)
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

        #region Implementation of ICryptoSigner

        public byte[] Sign(byte[] data, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public byte[] Sign(byte[] data, byte[] key)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var dataFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());
            var keyFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());
            var sigFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());

            try
            {
                // Сохраняем байтовый массив данных во временный файл со случайными именем
                File.WriteAllBytes(dataFileName, data);

                // Сохраняем байтовый массив ключа во временный файл со случайными именем
                File.WriteAllBytes(keyFileName, key);

                // Вызываем openssl и перехватываем stdout
                // openssl.cmd dgst -md_gost12_256 -sign key.pem test.txt > test.txt.sig
                var args = "dgst -" + _digestAlgorithmCode + " " +
                        "-sign " + keyFileName + " " +
                        dataFileName + " > " +
                        sigFileName;

                ExternalCommand.Call(_configurator.OpenSslCommand, args);

                // Считываем подпись из временного файла
                return File.ReadAllBytes(sigFileName);
            }
            finally
            {
                // Удаляем временные файлы
                if (File.Exists(dataFileName))
                {
                    File.Delete(dataFileName);
                }

                if (File.Exists(keyFileName))
                {
                    File.Delete(keyFileName);
                }

                if (File.Exists(sigFileName))
                {
                    File.Delete(sigFileName);
                }
            }
        }

        public byte[] SignHash(byte[] hash, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public byte[] SignHash(byte[] hash, byte[] key)
        {
            throw new NotImplementedException();
        }

        public bool Verify(byte[] data, byte[] signature, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public bool Verify(byte[] data, byte[] signature, byte[] key)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            // Сохраняем байтовый массив данных во временный файл со случайными именем
            var dataFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());
            var sigFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());
            var keyFileName = Path.Combine(_configurator.TempPath + Path.GetRandomFileName());

            try
            {
                File.WriteAllBytes(dataFileName, data);

                // Сохраняем байтовый массив подписи во временный файл со случайными именем
                File.WriteAllBytes(sigFileName, signature);

                // Сохраняем байтовый массив ключа во временный файл со случайными именем
                File.WriteAllBytes(keyFileName, key);

                // Вызываем openssl и перехватываем stdout
                // openssl.cmd dgst -md_gost12_256 -verify keypub.pem -signature test.txt.sig test.txt
                var args = "dgst -" + _digestAlgorithmCode + " " +
                        "-verify " + keyFileName + " " +
                        "-signature " + sigFileName + " " +
                        dataFileName;

                var stdout = ExternalCommand.Call(_configurator.OpenSslCommand, args);
                return stdout.TrimEnd() == "Verified OK";
            }
            finally
            {
                // Удаляем временные файлы
                if (File.Exists(dataFileName))
                {
                    File.Delete(dataFileName);
                }

                if (File.Exists(sigFileName))
                {
                    File.Delete(sigFileName);
                }

                if (File.Exists(keyFileName))
                {
                    File.Delete(keyFileName);
                }
            }
        }

        public bool VerifyHash(byte[] hash, byte[] signature, X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }

        public bool VerifyHash(byte[] hash, byte[] signature, byte[] key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
