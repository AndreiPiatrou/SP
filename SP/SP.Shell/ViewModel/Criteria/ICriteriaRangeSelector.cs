namespace SP.Shell.ViewModel.Criteria
{
    public interface ICriteriaRangeSelector
    {
        void Apply();

        bool CanApply();
    }
}