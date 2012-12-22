using System;
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
            //var episodes = drNuClient.GetEpisodes(new Uri("http://www.dr.dk/tv/program/matador"));

            //foreach (var episode in episodes)
            //{
            //    System.Console.WriteLine(episode.Uri);
            //}

            var uri = new Uri("http://www.dr.dk/TV/se/matador/matador-18-24");
            drNuClient.Download(uri);

            System.Console.ReadLine();
        }
    }
}