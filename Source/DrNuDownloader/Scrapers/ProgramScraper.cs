using System;
using System.IO;
using System.Text.RegularExpressions;
using DrNuDownloader.Wrappers;

namespace DrNuDownloader.Scrapers
{
    public class ProgramScraper : IScraper<Uri>
    {
        private readonly IWebRequestWrapper _webRequestWrapper;

        public ProgramScraper(IWebRequestWrapper webRequestWrapper)
        {
            if (webRequestWrapper == null) throw new ArgumentNullException("webRequestWrapper");

            _webRequestWrapper = webRequestWrapper;
        }

        public Uri Scrape(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var request = _webRequestWrapper.CreateHttp(programUri);
            var response = request.GetResponse();

            var streamReader = new StreamReader(response.GetResponseStream());
            var html = streamReader.ReadToEnd();

            var regex = new Regex("resource:\\s*\"(?<uri>.*)\"");
            var match = regex.Match(html);
            if (!match.Success)
            {
                throw new ScraperException("Unable to find resource.");
            }

            return new Uri(match.Groups["uri"].Value);
        }
    }
}