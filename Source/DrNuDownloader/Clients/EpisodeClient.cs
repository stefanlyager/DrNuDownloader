using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace DrNuDownloader.Clients
{
    public interface IEpisodeClient
    {
        void Download(Uri episodeUri);
    }

    public class EpisodeClient : IEpisodeClient
    {
        private readonly IResourceClient _resourceClient;
        private readonly IFileNameSanitizer _fileNameSanitizer;
        private readonly IDrNuRtmpStreamFactory _drNuRtmpStreamFactory;

        public EpisodeClient(IResourceClient resourceClient, IFileNameSanitizer fileNameSanitizer, IDrNuRtmpStreamFactory drNuRtmpStreamFactory)
        {
            if (resourceClient == null) throw new ArgumentNullException("resourceClient");
            if (fileNameSanitizer == null) throw new ArgumentNullException("fileNameSanitizer");
            if (drNuRtmpStreamFactory == null) throw new ArgumentNullException("drNuRtmpStreamFactory");

            _resourceClient = resourceClient;
            _fileNameSanitizer = fileNameSanitizer;
            _drNuRtmpStreamFactory = drNuRtmpStreamFactory;
        }

        public void Download(Uri episodeUri)
        {
            if (episodeUri == null) throw new ArgumentNullException("episodeUri");

            var request = WebRequest.CreateHttp(episodeUri);
            var response = request.GetResponse();

            var streamReader = new StreamReader(response.GetResponseStream());
            var html = streamReader.ReadToEnd();

            var regex = new Regex("resource:\\s*\"(?<uri>.*)\"");
            var match = regex.Match(html);
            if (!match.Success)
            {
                throw new ParseException("Unable to find resource.");
            }

            var resourceUri = new Uri(match.Groups["uri"].Value);
            var resource = _resourceClient.GetResource(resourceUri);

            var link = resource.Links.OrderByDescending(l => l.BitrateInKbps).First();
            var rtmpUri = new Uri(link.Uri);

            var fileName = _fileNameSanitizer.Sanitize(string.Format("{0}.flv", resource.PostingTitle));

            using (var drNuRtmpStream = _drNuRtmpStreamFactory.CreateDrNuRtmpStream(rtmpUri))
            {
                using (var fileStream = File.Create(fileName))
                {
                    drNuRtmpStream.CopyTo(fileStream);
                }
            }
        }
    }
}