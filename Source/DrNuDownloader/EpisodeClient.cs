using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DrNuDownloader
{
    public interface IEpisodeClient
    {
        void Download(Episode episode);
    }

    public class EpisodeClient : IEpisodeClient
    {
        private readonly IResourceClient _resourceClient;

        public EpisodeClient(IResourceClient resourceClient)
        {
            if (resourceClient == null) throw new ArgumentNullException("resourceClient");

            _resourceClient = resourceClient;
        }

        public void Download(Episode episode)
        {
            if (episode == null) throw new ArgumentNullException("episode");

            var request = WebRequest.CreateHttp(episode.Uri);
            var response = request.GetResponse();

            var streamReader = new StreamReader(response.GetResponseStream());
            var html = streamReader.ReadToEnd();

            var regex = new Regex("resource:\\s*\"(?<uri>.*)\"");
            var match = regex.Match(html);
            if (!match.Success)
            {
                throw new HtmlParseException("Unable to find resource.");
            }

            var resourceUri = new Uri(match.Groups["uri"].Value);

            var rtmpUri = _resourceClient.GetRtmpUri(resourceUri);
        }
    }
}