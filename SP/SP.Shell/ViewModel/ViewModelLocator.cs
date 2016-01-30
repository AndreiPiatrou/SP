using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.FIleSystem.Directory;
using SP.Shell.Services;
using SP.Shell.ViewModel.CommandListeners;

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

            // View models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SaveToFileCommandListener>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            SimpleIoc.Default.GetInstance<WorkingDirectory>().Dispose();
        }
    }
}