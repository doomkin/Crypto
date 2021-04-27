using System.Diagnostics;

namespace CryptoSSL.Helpers
{
    internal class ExternalCommand
    {
        public string Call(string fileName, string arguments)
        {
            var opensslCmd = new Process
                {
                    StartInfo =
                        {
                            FileName = fileName,
                            Arguments = arguments,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                };

            opensslCmd.Start();
            var stdout = opensslCmd.StandardOutput.ReadToEnd();
            opensslCmd.WaitForExit();
            return stdout;
        }
    }
}
