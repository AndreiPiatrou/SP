namespace SP.Shell.ViewModel
{
    public interface ICriteriaRangeSelector
    {
        void Apply();

        bool CanApply();
    }
}