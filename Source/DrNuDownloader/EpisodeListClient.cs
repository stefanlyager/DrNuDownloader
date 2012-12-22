using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace DrNuDownloader
{
    public interface IEpisodeListClient
    {
        IEnumerable<Episode> GetEpisodes(string programId);
    }

    public class EpisodeListClient : IEpisodeListClient
    {
        public IEnumerable<Episode> GetEpisodes(string programId)
        {
            if (programId == null) throw new ArgumentNullException("programId");

            var episodesUri = new Uri(string.Format("http://www.dr.dk/TV/se/episode/gallery/?programSeriesUrn={0}&mediaType=tv&skip=0&take=100", programId));
            var request = WebRequest.CreateHttp(episodesUri);
            var response = request.GetResponse();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.GetResponseStream());

            var containerArticleElement = htmlDocument.GetElementbyId(programId);
            if (containerArticleElement == null)
            {
                throw new ParseException(string.Format("Unable to find element with id {0}", programId));
            }

            var containerSectionElement = (from se in containerArticleElement.ChildNodes
                                           where se.Name == "section"
                                           select se).FirstOrDefault();
            if (containerSectionElement == null)
            {
                throw new ParseException("Unable to find section element.");
            }

            var articleElements = (from ae in containerSectionElement.ChildNodes
                                   where ae.Name == "article"
                                   select ae);
            foreach (var article in articleElements)
            {
                var aElement = (from ae in article.ChildNodes
                                where ae.Name == "a"
                                select ae).FirstOrDefault();
                if (aElement == null)
                {
                    continue;
                }

                var episodeUri = new Uri(string.Format("http://www.dr.dk{0}", aElement.Attributes["href"].Value));
                yield return new Episode
                                 {
                                     Uri = episodeUri
                                 };
            }
        }
    }
}