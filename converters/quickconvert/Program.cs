using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Converters;
using System.Xml;

namespace BarQuickExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Illegal use. Please read instructions silly.");
                Console.ReadKey();
            }

            Convert(args[0], "");
           
        }

        static void Convert(string file, string saveTo)
        {
            string ext = Path.GetExtension(file);

            switch (ext)
            {
                case ".bar":
                    extractbar(file);
                    Console.WriteLine("Done");
                    Console.ReadKey();
                    break;

                case ".bar2":
                    extractbar(file, true);
                    Console.WriteLine("Done");
                    Console.ReadKey();
                    break;

                case ".xmb":
                    convertXMB(file);
                    break;


                case ".xml":
                    convertXML(file);
                    break;

                default:
                    if (File.Exists("FileConverter.exe"))
                    {
                        Console.WriteLine("attempting AoE3ED");
                        System.Diagnostics.Process.Start("FileConverter.exe", string.Format("\"{0}\"", file));
                    }

                    else
                    {
                        Console.WriteLine("Unknown file, treating as XMB");
                        convertXMB(file);
                    }
                    break;
            }

        }

        static void convertXMB(string path)
        {
            XMBFile xmb = new XMBFile(path);
            string newfilename = String.Format("{0}", Path.ChangeExtension(path, "xml"));
            Console.WriteLine(newfilename);
            xmb.SaveAsXML(newfilename);
        }

        static void convertXML(string path)
        {
            string newfilename = String.Format("{0}", Path.ChangeExtension(path, "xmb"));
            Console.WriteLine(newfilename);
            XmlDocument x = new XmlDocument();
            x.Load(path);
            XMLFile.toXMB(x, newfilename);
        }

        static void extractbar(string path)
        {
            extractbar(path, false);
        }

        static void extractbar(string path, bool autoconvert)
        {
            string p = String.Format("{0}/{1}", Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
            extractbar(path, p, autoconvert);
        }
        static void extractbar(string path, string saveTo, bool autoconvert)
        {
            Console.WriteLine("Converting BAR file (extract to directory): {0}", path);
            BARFile barfile = new BARFile(path);
            
            DirectoryInfo newpath = Directory.CreateDirectory(saveTo);

            foreach (BAREntry e in barfile)
            {
                string filepath = string.Format("{0}/{1}", newpath.FullName, e.filename);

                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));

                using (Stream file = File.OpenWrite(filepath))
                {
                    CopyStream(barfile.ExtractFileStream(e), file);
                }
                Console.WriteLine("Extracted {0}",filepath);

                if(autoconvert)
                {
                    Convert(filepath, saveTo);
                }
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
