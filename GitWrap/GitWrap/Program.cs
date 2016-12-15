using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitWrap
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            // Bash.exe
            ProcessStartInfo bashInfo = new ProcessStartInfo();
            bashInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            @"System32\bash.exe");

            // Loop through args and pass them to git executable
            String argsString = "-c \" git";
            for (int i = 0; i < args.Length; i++)
            {
                // Translate directory structure.
                // Note: Currently only handles C: drive (hardcoded value)
                String argstr = args[i].Replace("C:\\", "/mnt/c/");
                // Convert Windows path to Linux style paths
                argstr = argstr.Replace("\\", "/");
                argsString += " " + argstr;
            }
            string oFilename = "gitwrap_output_" + RandomString(12);
            argsString += "> /tmp/" + oFilename + " && chmod 777 /tmp/" + oFilename + "\"";
            bashInfo.Arguments = argsString;
            bashInfo.UseShellExecute = false;
            bashInfo.RedirectStandardOutput = false;
            bashInfo.RedirectStandardError = false;
            bashInfo.CreateNoWindow = true;

            var proc = new Process
            {
                StartInfo = bashInfo
            };
            proc.Start();
            proc.WaitForExit();

            string outputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @"AppData\Local\lxss\rootfs\tmp\"+oFilename);
            if (System.IO.File.Exists(outputFilePath))
            {
                //File.SetAttributes(outputFilePath, FileAttributes.Normal);
                string text = System.IO.File.ReadAllText(outputFilePath);
                text.Replace("\r\n", "\n");
                System.Console.WriteLine(text);
                System.IO.File.Delete(outputFilePath);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
