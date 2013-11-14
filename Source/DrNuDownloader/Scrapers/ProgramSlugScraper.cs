using System;
using System.IO;
using System.Text.RegularExpressions;
using DrNuDownloader.Wrappers;

namespace DrNuDownloader.Scrapers
{
    public interface IProgramSlugScraper : IScraper<Uri, Slug> { }

    public class ProgramSlugScraper : IProgramSlugScraper
    {
        private readonly IWebRequestWrapper _webRequestWrapper;

        public ProgramSlugScraper(IWebRequestWrapper webRequestWrapper)
        {
            if (webRequestWrapper == null) throw new ArgumentNullException("webRequestWrapper");

            _webRequestWrapper = webRequestWrapper;
        }

        public Slug Scrape(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");
            if (!programUri.IsAbsoluteUri) throw new ArgumentException("Only absolute URIs are supported.", "programUri");

            var request = _webRequestWrapper.CreateHttp(programUri);
            var response = request.GetResponse();

            string html;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                html = streamReader.ReadToEnd();
            }

            var regex = new Regex("programSerieSlug:\\s*\"(?<slug>.*)\"");
            var match = regex.Match(html);
            if (!match.Success)
            {
                throw new ScraperException("Unable to find programSerieSlug.");
            }

            return match.Groups["slug"].Value;
        }
    }
}