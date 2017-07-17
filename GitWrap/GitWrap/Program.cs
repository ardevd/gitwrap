using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            String argsString = "-c \"git";
            for (int i = 0; i < args.Length; i++)
            {
                // Translate directory structure.
                // Use regex to translate drive letters.
                string pattern = @"(\D):\\";
                String argstr = args[i];
                foreach (Match match in Regex.Matches(args[i], pattern, RegexOptions.IgnoreCase))
                {
                    string driveLetter = match.Groups[1].ToString();
                    argstr = Regex.Replace(args[i], pattern, "/mnt/" + driveLetter.ToLower() + "/");
                }
               
                // Convert Windows path to Linux style paths
                argstr = argstr.Replace("\\", "/");
                argsString += " " + argstr;
            }

            argsString += "\"";
            bashInfo.Arguments = argsString;
            bashInfo.UseShellExecute = false;
            bashInfo.RedirectStandardOutput = true;
            bashInfo.RedirectStandardError = true;
            bashInfo.CreateNoWindow = true;

            var proc = new Process
            {
                StartInfo = bashInfo
            };

            proc.OutputDataReceived += CaptureOutput;
            proc.ErrorDataReceived += CaptureError;

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
        }

        static void CaptureOutput(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.Length > 1)
            {
                String outputString = e.Data.Replace("\n", Environment.NewLine);
                Console.WriteLine(outputString);
            }
        }

        static void CaptureError(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
