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

            argsString += "> /tmp/gitwrap_output\"";
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
                @"AppData\Local\lxss\rootfs\tmp\gitwrap_output");
            if (System.IO.File.Exists(outputFilePath))
            {
                string text = System.IO.File.ReadAllText(outputFilePath);
                text.Replace("\r\n", "\n");
                System.Console.WriteLine(text);
                System.IO.File.Delete(outputFilePath);
            }
        }
    }
}
