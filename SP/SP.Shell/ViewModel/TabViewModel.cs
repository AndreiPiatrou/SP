namespace SP.Shell.ViewModel
{
    public class TabViewModel
    {
        public TabViewModel(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}
