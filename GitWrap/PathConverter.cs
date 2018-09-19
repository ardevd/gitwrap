using System;
using System.Text.RegularExpressions;

namespace GitWrap
{
    public class PathConverter
    {
        // Convert Windows style paths submitted to GitWrap to their Linux style equivalent
        public static string convertPathFromWindowsToLinux(String path)
        {
            // Translate directory structure.
            // Use regex to translate drive letters.
            string pattern = @"(\D):\\";
            string argstr = path;
            foreach (Match match in Regex.Matches(path, pattern, RegexOptions.IgnoreCase))
            {
                string driveLetter = match.Groups[1].ToString();
                argstr = Regex.Replace(path, pattern, Properties.Settings.Default.wslpath + driveLetter.ToLower() + "/");
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

       // Convert output paths from WSL git to Windows style equivalent.
       // For now we only convert forward slashes to backslashes if our regex pattern for
       // absolute paths get a match. If not we will end up converting urls, and other stuff too.
       public static string convertPathFromLinuxToWindows(String path) 
        {
            bool foundDrivePath = false;
            string drivePathPattern = String.Format(@"{0}(\D)/", Properties.Settings.Default.wslpath);
            string argstr = path;

            foreach (Match match in Regex.Matches(path, drivePathPattern, RegexOptions.None))
            {
                string driveLetter = match.Groups[1].ToString();
                argstr = Regex.Replace(path, drivePathPattern, driveLetter.ToUpper() + ":\\");
                // Convert Linux style path separators to Windows style equivalent
                argstr = argstr.Replace("/", "\\");
                foundDrivePath = true;
            }

            if (foundDrivePath)
            {
                argstr = argstr.Replace("/", "\\");
            }
            return argstr;
        }

    }
}
