using SP.Shell.Models;

namespace SP.Shell.Messages
{
    public class SaveRecordsToFileMessage
    {
        public SaveRecordsToFileMessage(RecordsCollection records, string filePath)
        {
            Records = records;
            FilePath = filePath;
        }

        public RecordsCollection Records { get; private set; }

        public string FilePath { get; private set; }
    }
}
