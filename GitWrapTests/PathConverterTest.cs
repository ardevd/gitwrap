using Microsoft.VisualStudio.TestTools.UnitTesting;
using GitWrap;

namespace GitWrapTests
{
    [TestClass]
    public class PathConverterTest
    {
        [TestMethod]
        public void TestConvertWindowsPathToWSLEquivalent()
        {
            // Example windows path 
            string windowsPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\testfile.txt";
            // The equivalent WSL path
            string unixPath = "/mnt/c/Program\\ Files\\ \\(x86\\)/Microsoft\\ Visual\\ Studio/testfile.txt";
            // assert that the converter method correctly converts the Windows path to its WSL style equivalent.
            Assert.AreEqual(unixPath, PathConverter.convertPathFromWindowsToLinux(windowsPath), false, "Windows path converted correctly to WSL equivalent.");
        }

        [TestMethod]
        public void TestConvertWSLPathToWindowsEquivalent()
        {
            // Example WSL Path
            string unixPath = "/mnt/c/Program Files (x86)/Microsoft Visual Studio/testfile.txt";

            // Equivalent Windows path
            string windowsPath = "C:\\Program Files (x86)\\Microsoft Visual Studio\\testfile.txt";

            string convertedPath = PathConverter.convertPathFromLinuxToWindows(unixPath);

            // assert correct conversion
            Assert.AreEqual(unixPath, PathConverter.convertPathFromLinuxToWindows(unixPath), false, "WSL path converted correctly to Windows equivalent.");
        }
    }
}
