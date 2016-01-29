namespace SP.Shell.Models
{
    public class CheckableHeaderModel
    {
        public CheckableHeaderModel(string header, int index)
        {
            Header = header;
            Index = index;
        }

        public string Header { get; private set; }

        public int Index { get; private set; }

        public bool IsChecked { get; set; }
    }
}
