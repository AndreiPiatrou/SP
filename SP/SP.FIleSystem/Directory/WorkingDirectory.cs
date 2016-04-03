using System;
using System.IO;

namespace SP.FIleSystem.Directory
{
    public class WorkingDirectory : IDisposable
    {
        private readonly string directory;

        public WorkingDirectory(string root)
        {
            directory = CreateDirectory(root);
        }

        public void Dispose()
        {
            System.IO.Directory.Delete(directory, true);
        }

        public string WriteToNewFile(string content, string extension = ".csv")
        {
            var newFIlePath = GenerateNewFilePath(extension);

            return CreateFileWithContent(newFIlePath, content);
        }

        public string GenerateNewFilePath(string extension = ".csv")
        {
            var name = Guid.NewGuid() + extension;

            return PathOperations.Combine(directory, name);
        }

        private string CreateFileWithContent(string path, string content)
        {
            File.WriteAllText(path, content);

            return path;
        }

        private string CreateDirectory(string root)
        {
            if (root == "%Temp%")
            {
                root = Path.GetTempPath();
            }

            var name = Guid.NewGuid().ToString();

            return System.IO.Directory.CreateDirectory(PathOperations.Combine(root, name)).FullName;
        }
    }
}
