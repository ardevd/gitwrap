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
        private static int outputLines = 0;

        static void Main(string[] args)
        {
            executeGitWithArgs(getBashPath(), args);   
        }

        static void executeGitWithArgs(String bashPath, string[] args)
        {
            if (!File.Exists(bashPath))
            {
                Console.Write("[-] Error: Bash.exe not found.");
                return;
            }

            ProcessStartInfo bashInfo = new ProcessStartInfo();
            bashInfo.FileName = bashPath;

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
                // Escape any whitespaces
                argstr = argstr.Replace(" ", "\\ ");
                argsString += " " + argstr;
            }

            // Append quotation to close of the argument supplied to bash.exe
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
            printOutputData(e.Data);
        }

        static void CaptureError(object sender, DataReceivedEventArgs e)
        {
            printOutputData(e.Data);
        }

        static String getBashPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            @"System32\bash.exe");
        }

        static void printOutputData(String data)
        {
            if (data != null)
            {
                if (outputLines > 0)
                {
                    Console.Write(Environment.NewLine);
                }
                Console.Write(data);
                outputLines++;
            }
        }
    }
}
