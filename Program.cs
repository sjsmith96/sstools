using System;
using System.IO;
using PgpCore;

namespace sstools
{

    public class SSEncrypt
    {
        public SSEncrypt()
        {
        }
        
        private const string TEST_KEY_PATH = @"C:\users\sam\desktop\test.asc";
        private const string TEST_INPUT_PATH = @"C:\users\sam\desktop\pgptest.zip";
        private const string TEST_OUTPUT_PATH = @"C:\users\sam\desktop\testes.pgp";
        
        public void PgpEncrypt()
        {
            FileInfo publicKey = new FileInfo(TEST_KEY_PATH);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            FileInfo inputFile = new FileInfo(TEST_INPUT_PATH);
            FileInfo outputFile = new FileInfo(TEST_OUTPUT_PATH);

            PGP pgp = new PGP(encryptionKeys);
            pgp.EncryptFile(inputFile, outputFile);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            SSEncrypt test = new SSEncrypt();
            test.PgpEncrypt();
        }
    }
}
