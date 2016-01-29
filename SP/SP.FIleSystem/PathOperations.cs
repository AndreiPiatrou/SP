using System.Diagnostics;
using System.IO;

namespace SP.FIleSystem
{
    public class PathOperations
    {
        public static string AppPathCombine(string relativePath)
        {
            var appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            return Combine(appPath, relativePath);
        }

        public static string Combine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }
    }
}
