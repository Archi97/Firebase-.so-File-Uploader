using System;
using System.Diagnostics;
using System.IO;

namespace Firebase_.so_File_Uploader
{
    class Program
    {
        //WARNING. 
        //Put your appID before lunching the application
        static string appID = "";
        static string firebaseCommand = "firebase crashlytics:symbols:upload --app=" + appID + " ";// + path

        static void Main(string[] args)
        {
            bool pathfound = false;
            string path = "";

            while (!pathfound)
            {
                Console.WriteLine("Put folder path to find all *.so files in the directory and subdirectories...");
                path = Console.ReadLine();
                if (Directory.Exists(path))
                    pathfound = true;
                else
                    Console.WriteLine("||||| Directory not found |||||");
            }

            RekusrsiveFileCheck(path);
            Console.ReadLine();
        }

        static int filecount = 0;
        public static void RekusrsiveFileCheck(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            DirectoryInfo[] d_Info = d.GetDirectories();
            FileInfo[] f_infos = d.GetFiles();

            if (d_Info != null && d_Info.Length > 0)
            {
                foreach (var item in d_Info)
                {
                    RekusrsiveFileCheck(item.FullName);
                }
            }

            foreach (var item in f_infos)
            {
                if (item.Name.Contains(".so") && !item.Name.Contains(".meta"))
                {
                    filecount++;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n\n============== FILE FOUND " + filecount + " ==============");
                    Console.WriteLine(item.FullName);
                    Process proc = Process.Start(@"cmd", @"/c " + firebaseCommand + "\"" + item.FullName + "\"");
                    proc.WaitForExit();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("============== UPLOADED  ==============");

                }
            }
        }
    }
}
