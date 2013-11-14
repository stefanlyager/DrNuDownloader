using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using DrNuDownloader.Wrappers;

namespace DrNuDownloader.Scrapers
{
    public interface IResourceUriScraper : IScraper<Uri, Uri> { }

    public class ResourceUriScraper : IResourceUriScraper
    {
        private readonly IWebRequestWrapper _webRequestWrapper;

        public ResourceUriScraper(IWebRequestWrapper webRequestWrapper)
        {
            if (webRequestWrapper == null) throw new ArgumentNullException("webRequestWrapper");

            _webRequestWrapper = webRequestWrapper;
        }

        public Uri Scrape(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");
            if (!programUri.IsAbsoluteUri) throw new ArgumentException("Only absolute URIs are supported.", "programUri");

            try
            {
                var request = _webRequestWrapper.CreateHttp(programUri);
                var response = request.GetResponse();

                string html;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    html = streamReader.ReadToEnd();
                }

                var regex = new Regex("resource:\\s*\"(?<uri>.*)\"");
                var match = regex.Match(html);
                if (!match.Success)
                {
                    throw new ScraperException("Unable to find resource.");
                }

                return new Uri(match.Groups["uri"].Value);
            }
            catch (WebException exception)
            {
                throw new ScraperException("Unable to find resource.", exception);
            }
        }
    }
}