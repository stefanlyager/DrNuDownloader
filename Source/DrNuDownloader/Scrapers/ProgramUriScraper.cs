using System;
using System.Collections.Generic;
using System.Linq;
using DrNuDownloader.Wrappers;
using HtmlAgilityPack;

namespace DrNuDownloader.Scrapers
{
    public interface IProgramUriScraper : IScraper<Slug, IEnumerable<Uri>> { }

    public class ProgramUriScraper : IProgramUriScraper
    {
        private readonly IWebRequestWrapper _webRequestWrapper;

        public ProgramUriScraper(IWebRequestWrapper webRequestWrapper)
        {
            if (webRequestWrapper == null) throw new ArgumentNullException("webRequestWrapper");

            _webRequestWrapper = webRequestWrapper;
        }

        public IEnumerable<Uri> Scrape(Slug slug)
        {
            if (slug == null) throw new ArgumentNullException("slug");
            
            var episodesUri = new Uri(string.Format("http://www.dr.dk/TV/play/AllEpisodes?slug={0}&episodesperpage=100&pagenumber=1", slug));
            var request = _webRequestWrapper.CreateHttp(episodesUri);
            var response = request.GetResponse();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.GetResponseStream());

            var liElements = htmlDocument.DocumentNode.SelectNodes("//li");
            if (liElements == null)
            {
                throw new ScraperException("No li elements found.");
            }

            var uris = new List<Uri>();
            foreach (var liElement in liElements)
            {
                var aElement = (from ae in liElement.ChildNodes
                                where ae.Name == "a"
                                select ae).FirstOrDefault();
                if (aElement == null || aElement.Attributes["href"] == null)
                {
                    continue;
                }

                uris.Add(new Uri(string.Format("http://www.dr.dk{0}", aElement.Attributes["href"].Value)));
            }

            if (!uris.Any())
            {
                throw new ScraperException("No program URIs found.");
            }

            return uris;
        }
    }
}