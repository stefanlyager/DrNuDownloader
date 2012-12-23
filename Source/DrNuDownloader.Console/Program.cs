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
            if (arguments.l != null)
            {
                var episodes = drNuClient.GetEpisodes(arguments.l);
                foreach (var episode in episodes)
                {
                    System.Console.WriteLine(episode.Uri);
                }
            }
            else if (arguments.d != null)
            {
                drNuClient.Download(arguments.d);
            }
            else if (arguments.da != null)
            {
                var episodes = drNuClient.GetEpisodes(arguments.da);
                foreach (var episode in episodes)
                {
                    drNuClient.Download(episode.Uri);
                }
            }
            else
            {
                System.Console.WriteLine("Invalid argument.");
            }

            System.Console.ReadLine();
        }
    }
}