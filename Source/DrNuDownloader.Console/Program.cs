using System;

namespace DrNuDownloader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var drNuClient = new DrNuClient(new ProgramClient(), new EpisodeListClient());
            var episodes = drNuClient.GetEpisodes(new Uri("http://www.dr.dk/tv/program/matador"));

            foreach (var episode in episodes)
            {
                System.Console.WriteLine("This is an episode...");
            }

            System.Console.ReadLine();
        }
    }
}