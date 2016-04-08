using SP.Shell.Models;

namespace SP.Shell.Messages
{
    public class DataGridSelectionChangedMessage
    {
        public DataGridSelectionChangedMessage(RecordsCollection records)
        {
            Records = records;
        }

        public RecordsCollection Records { get; private set; }
    }
}
