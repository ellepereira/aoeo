using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zlib;
using System.Net;
using System.Window­s.Forms;

namespace ConsoleApplication1
{

    class Program
    {
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static string getpagecontent(string URL)
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create(URL);

            HttpWebResponse response;

            try
            {
                // execute the request
                response = (HttpWebResponse)
                    request.GetResponse();
            }
            catch
            {
                return null;
            }

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            // print out page source
            return sb.ToString();
        }

        public static void findbuild(int min, int max, string type)
        {
            int current = min;

            while (current <= max)
            {
                string page = string.Format("http://spartan.msgamestudios.com/content/spartan/{0}/{1}/manifest.bin", type, current);
                string result = getpagecontent(page);
                if (result != null && string.Compare("File not found.", result) != 0)
                {
                    Console.WriteLine("Found build: {0}", current);
                    Console.WriteLine("Press enter to continue", current);
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Not: {0}", current);
                }

                current++;
            }

        }

        public static void compress()
        {
            FileStream f = File.Open("C:/users/Luciano/Desktop/manifest.txt", FileMode.Open, FileAccess.ReadWrite);
            FileStream final = File.Create("C:/users/Luciano/Desktop/manifest.bin");

            int size = (int)f.Length;

            StreamReader r = new StreamReader(f);
            BinaryWriter w = new BinaryWriter(final);

            w.Write("l33t".ToCharArray());
            w.Write(size);
            w.BaseStream.WriteByte(0x78);
            w.BaseStream.WriteByte(0x9C);

            DeflateStream a = new DeflateStream(f, CompressionMode.Compress);

            int read = 0;
            byte[] buffer = new byte[1024 * 1024];

            while ((read = a.Read(buffer, 0, buffer.Length)) > 0)
            {
                w.Write(buffer, 0, read);
            }

            a.Close();
            f.Close();
            

        }

        /*static void decompress()
        {
            decompress("C:/users/Luciano/Desktop/manifest.bin");
        }*/

        static void decompress(string inputfile, string outputPath)
        {
            FileStream f = File.Open(inputfile, FileMode.Open);
            FileStream final = File.Create(outputPath);

            StreamReader reader = new StreamReader(f);
            StreamWriter writer = new StreamWriter(final);

            DeflateStream a = new DeflateStream(f, CompressionMode.Decompress);

            int read = 0;
            byte[] buffer = new byte[1024 * 1024];

            f.Position = 10;

            while ((read = a.Read(buffer, 0, buffer.Length)) > 0)
            {
                final.Write(buffer, 0, read);
            }

            f.Close();
            a.Close();
            final.Close();
        }

        static void DownloadAndDecompress(string URI, string filename)
        {
            string fileName = filename;
            string appDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string savePath = appDir + "\\" + filename;
            string downloadPath = savePath + ".bin";
            
            // Create a new WebClient instance.
            WebClient myWebClient = new WebClient();
            // Concatenate the domain with the Web resource filename.
            Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, URI);
            // Download the Web resource and save it into the current filesystem folder.
            myWebClient.DownloadFile(URI, downloadPath);
            Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, URI);
            Console.WriteLine("\nDownloaded file saved in:\n\t" + appDir);
            Console.WriteLine("Decompressing: " + appDir+"\\"+filename);
            decompress(downloadPath, savePath);
        }

        static void Main(string[] args)
        {
            DownloadAndDecompress("http://spartan.msgamestudios.com/content/spartan/precert/2144/P0214400011.bin", "Launcher.exe");
            Console.ReadKey();

            //decompress();
            //compress();
            /*
            if (args.Length == 3)
                findbuild(int.Parse(args[0]), int.Parse(args[1]), args[2]);
            if (args.Length == 2)
                findbuild(int.Parse(args[0]), int.Parse(args[1]), "production");
            else
                findbuild(5336, 10000, "cert");*/
        }
    }
}
