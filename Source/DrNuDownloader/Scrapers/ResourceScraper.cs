using System;
using System.IO;
using DrNuDownloader.Scrapers.Json;
using DrNuDownloader.Wrappers;

namespace DrNuDownloader.Scrapers
{
    public interface IResourceScraper : IScraper<Uri, Resource> { }

    public class ResourceScraper : IResourceScraper
    {
        private readonly IWebRequestWrapper _webRequestWrapper;
        private readonly IJsonConvertWrapper _jsonConvertWrapper;

        public ResourceScraper(IWebRequestWrapper webRequestWrapper, IJsonConvertWrapper jsonConvertWrapper)
        {
            if (webRequestWrapper == null) throw new ArgumentNullException("webRequestWrapper");
            if (jsonConvertWrapper == null) throw new ArgumentNullException("jsonConvertWrapper");

            _webRequestWrapper = webRequestWrapper;
            _jsonConvertWrapper = jsonConvertWrapper;
        }

        public Resource Scrape(Uri resourceUri)
        {
            if (resourceUri == null) throw new ArgumentNullException("resourceUri");

            var request = _webRequestWrapper.CreateHttp(resourceUri);
            var response = request.GetResponse();

            string json;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            return _jsonConvertWrapper.DeserializeObject<Resource>(json);
        }
    }
}