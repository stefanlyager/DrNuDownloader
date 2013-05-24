namespace DrNuDownloader.Wpf.ViewModel
{
    public interface IMainWindowViewModel
    {
        IDownloadViewModel DownloadViewModel { get; }
    }

    public class MainWindowViewModel : IMainWindowViewModel
    {
        public IDownloadViewModel DownloadViewModel { get; private set; }

        public MainWindowViewModel(IDownloadViewModel downloadViewModel)
        {
            DownloadViewModel = downloadViewModel;
        }
    }
}