using System;
using System.Text.RegularExpressions;

namespace GitWrap
{
    public class PathConverter
    {

        public static string convertPathFromWindowsToLinux(String path)
        {
            // Translate directory structure.
            // Use regex to translate drive letters.
            string pattern = @"(\D):\\";
            String argstr = path;
            foreach (Match match in Regex.Matches(path, pattern, RegexOptions.IgnoreCase))
            {
                string driveLetter = match.Groups[1].ToString();
                argstr = Regex.Replace(path, pattern, "/mnt/" + driveLetter.ToLower() + "/");
            }

            // Convert Windows path to Linux style paths
            argstr = argstr.Replace("\\", "/");
            // Escape any whitespaces
            argstr = argstr.Replace(" ", "\\ ");
            // Escape parenthesis
            argstr = argstr.Replace("(", "\\(");
            argstr = argstr.Replace(")", "\\)");
            return argstr;
        }
    }
}
