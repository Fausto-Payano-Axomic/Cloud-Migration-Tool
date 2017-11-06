using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;

namespace Cloud_Migration_Tool
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<Cloud_Migration_Tool.ViewModels.MigrationViewModel>();
        }

        public Cloud_Migration_Tool.ViewModels.MigrationViewModel MainWindow => ServiceLocator.Current.GetInstance<Cloud_Migration_Tool.ViewModels.MigrationViewModel>();
    }
}
