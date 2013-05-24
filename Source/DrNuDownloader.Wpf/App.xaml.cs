using System.Windows;
using Autofac;
using DrNuDownloader.Wpf.ViewModel;

namespace DrNuDownloader.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            var mainWindowViewModel = bootstrapper.Container.Resolve<IMainWindowViewModel>();
            var mainWindow = new MainWindow
                                 {
                                     DataContext = mainWindowViewModel
                                 };
            mainWindow.Show();
        }
    }
}