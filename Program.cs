using System;
using System.IO;
using PgpCore;

namespace sstools
{
    public class SSTools
    {
        public SSTools()
        { }
        
        public void PgpEncrypt(string keyPath, string input, string output)
        {
            FileInfo publicKey = new FileInfo(keyPath);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            FileInfo inputFile = new FileInfo(input);
            FileInfo outputFile = new FileInfo(output);

            PGP pgp = new PGP(encryptionKeys);
            pgp.EncryptFile(inputFile, outputFile);
        }
    }
    
    class Program
    {                            
        private const string TEST_KEY_PATH = @"C:\users\sam\desktop\test.asc";
        private const string TEST_INPUT_PATH = @"C:\users\sam\desktop\pgptest.zip";
        private const string TEST_OUTPUT_PATH = @"C:\users\sam\desktop\testes.pgp";
        
        static void Main(string[] args)
        {
            SSTools tools = new SSTools();
            tools.PgpEncrypt(TEST_KEY_PATH, TEST_INPUT_PATH, TEST_OUTPUT_PATH);
        }
    }
}
