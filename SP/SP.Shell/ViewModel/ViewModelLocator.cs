using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.FIleSystem.Directory;
using SP.Resources;
using SP.Shell.Services;
using SP.Shell.ViewModel.CommandListeners;
using SP.Shell.ViewModel.Criteria;

namespace SP.Shell.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            // Services
            SimpleIoc.Default.Register<Settings.Settings>();
            SimpleIoc.Default.Register<Messenger>();
            SimpleIoc.Default.Register<DataReadService>();
            SimpleIoc.Default.Register(() => new WorkingDirectory(SimpleIoc.Default.GetInstance<Settings.Settings>().RootWorkingDirectoryPath));
            SimpleIoc.Default.Register<AnalysisService>();
            SimpleIoc.Default.Register<DataWriteService>();
            SimpleIoc.Default.Register<TabNameService>();

            // View models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CriteriaRangeViewModel>();

            // Listeners
            SimpleIoc.Default.Register<SaveToFileCommandListener>(true);
            SimpleIoc.Default.Register<AnalyzeDataCommandListener>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CriteriaRangeViewModel CriteriaRange
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CriteriaRangeViewModel>();
            }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.GetInstance<WorkingDirectory>().Dispose();
        }
    }
}