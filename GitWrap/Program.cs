using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GitWrap
{
    class Program
    {
        private static int outputLines = 0;

        static void Main(string[] args)
        {
            executeGitWithArgs(getWslPath(), args);   
        }

        static void executeGitWithArgs(String wslPath, string[] args)
        {
            if (!File.Exists(wslPath))
            {
                Console.Write("[-] Error: wsl.exe not found.");
                return;
            }

            ProcessStartInfo bashInfo = new ProcessStartInfo();
            bashInfo.FileName = wslPath;

            // Loop through args and pass them to git executable
            StringBuilder argsBld = new StringBuilder();
            argsBld.Append("git");

            for (int i = 0; i < args.Length; i++)
            {
                argsBld.Append(" " + PathConverter.convertPathFromWindowsToLinux(args[i]));
            }

            bashInfo.Arguments = argsBld.ToString();
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

        static String getWslPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            @"System32\wsl.exe");
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
