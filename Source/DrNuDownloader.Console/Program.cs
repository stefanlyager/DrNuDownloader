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

            var episode = new Episode
                              {
                                  Uri = new Uri("http://www.dr.dk/tv/se/matador/matador-16-24")
                              };
            drNuClient.Download(episode);

            System.Console.ReadLine();
        }
    }
}