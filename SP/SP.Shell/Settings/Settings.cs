using System.Configuration;

namespace SP.Shell.Settings
{
    public class Settings
    {
        public Settings()
        {
            RootWorkingDirectoryPath = ReadString("RootWorkingDirectoryPath");
        }

        public string RootWorkingDirectoryPath { get; private set; }

        private string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
