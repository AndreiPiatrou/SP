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

        private string CreateFileWithContent(string path, string content)
        {
            File.WriteAllText(path, content);

            return path;
        }

        private string GenerateNewFilePath(string extension)
        {
            var name = Guid.NewGuid() + extension;

            return PathOperations.Combine(directory, name);
        }

        private string CreateDirectory(string root)
        {
            var name = Guid.NewGuid().ToString();

            return System.IO.Directory.CreateDirectory(PathOperations.Combine(root, name)).FullName;
        }
    }
}
