using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace DrNuDownloader.Clients
{
    public interface IEpisodeListClient
    {
        IEnumerable<Uri> GetEpisodeUris(string slug);
    }

    public class EpisodeListClient : IEpisodeListClient
    {
        public IEnumerable<Uri> GetEpisodeUris(string slug)
        {
            if (slug == null) throw new ArgumentNullException("slug");

            var episodesUri = new Uri(string.Format("http://www.dr.dk/TV/play/AllEpisodes?slug={0}&episodesperpage=100&pagenumber=1", slug));
            var request = WebRequest.CreateHttp(episodesUri);
            var response = request.GetResponse();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.GetResponseStream());

            var liElements = htmlDocument.DocumentNode.SelectNodes("//li");
            if (liElements == null)
            {
                throw new ParseException("Unable to find li elements.");
            }

            foreach (var liElement in liElements)
            {
                var aElement = (from ae in liElement.ChildNodes
                                where ae.Name == "a"
                                select ae).FirstOrDefault();
                if (aElement == null)
                {
                    continue;
                }

                yield return new Uri(string.Format("http://www.dr.dk{0}", aElement.Attributes["href"].Value));
            }
        }
    }
}