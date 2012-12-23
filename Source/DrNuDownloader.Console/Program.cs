using Autofac;

namespace DrNuDownloader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            var drNuClient = bootstrapper.Container.Resolve<IDrNuClient>();

            var arguments = Args.Configuration.Configure<Arguments>().CreateAndBind(args);
            if (arguments.ListUri != null)
            {
                var episodeUris = drNuClient.GetEpisodeUris(arguments.ListUri);
                foreach (var uri in episodeUris)
                {
                    System.Console.WriteLine(uri);
                }
            }
            else if (arguments.DownloadUri != null)
            {
                drNuClient.Download(arguments.DownloadUri);
            }
            else if (arguments.DownloadAllUri != null)
            {
                var episodeUris = drNuClient.GetEpisodeUris(arguments.DownloadAllUri);
                foreach (var uri in episodeUris)
                {
                    drNuClient.Download(uri);
                }
            }
            else
            {
                System.Console.WriteLine("Invalid argument.");
            }
        }
    }
}