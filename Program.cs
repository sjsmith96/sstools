using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PgpCore;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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

        // TODO: Returns the average distance of the top 10% of matches. I have no idea if this is a viable way to do this.
        private float testImageAgainstTemplate(string image, ORBDetector orb, BFMatcher bf)
        {
            Mat img = CvInvoke.Imread(image, LoadImageType.AnyColor);

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            Mat descriptors = new Mat();
            orb.DetectAndCompute(img, null, keypoints, descriptors, false);

            VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
            bf.KnnMatch(descriptors, matches, 1, null);

            List<MDMatch[]> filteredMatches = new List<MDMatch[]>();
            
            MDMatch[][] matchArrayJagged = matches.ToArrayOfArray();
            List<float> distances = new List<float>();
            for(int i = 0; i < matchArrayJagged.Length; i++)
            {
                distances.Add(matchArrayJagged[i][0].Distance);
            }
            

            return distances.Average();
        }

        public void GetBestMatchingImage(string template, string[] images)
        {
            Mat templateImg = CvInvoke.Imread(template, LoadImageType.AnyColor);
            Console.WriteLine("Width {0}, Height {1}", templateImg.Width, templateImg.Height);
            ORBDetector orb = new ORBDetector(1000);

            VectorOfKeyPoint templateKeypoints = new VectorOfKeyPoint();
            Mat templateDescriptors = new Mat();
            orb.DetectAndCompute(templateImg, null, templateKeypoints, templateDescriptors, false);

            BFMatcher bf = new BFMatcher(DistanceType.Hamming, false);
            bf.Add(templateDescriptors);

            Console.WriteLine("Average Distance: {0}\n", testImageAgainstTemplate(images[0], orb, bf));
        }
        
    }
    
    class Program
    {                            
        private const string TEST_KEY_PATH = @"C:\users\sam\desktop\test.asc";
        private const string TEST_INPUT_PATH = @"C:\users\sam\desktop\pgptest.zip";
        private const string TEST_OUTPUT_PATH = @"C:\users\sam\desktop\testes.pgp";

        private const string CV_TEST_TEMPLATE_PATH = @"C:\qc\hcfa_blank.png";
        private const string CV_TEST_PATH = @"C:\qc\478829012.png";
        
        static void Main(string[] args)
        {
            
            SSTools tools = new SSTools();
            //tools.PgpEncrypt(TEST_KEY_PATH, TEST_INPUT_PATH, TEST_OUTPUT_PATH);
            string[] images = { CV_TEST_PATH };
            tools.GetBestMatchingImage(CV_TEST_TEMPLATE_PATH, images);
        }
        
    }
}
