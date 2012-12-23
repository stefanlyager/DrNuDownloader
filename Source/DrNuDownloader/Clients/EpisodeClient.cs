using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using DrNuDownloader.Util;

namespace DrNuDownloader.Clients
{
    public interface IEpisodeClient
    {
        void Download(Uri episodeUri);
    }

    public class EpisodeClient : IEpisodeClient
    {
        private readonly IResourceClient _resourceClient;
        private readonly IRtmpDump _rtmpDump;
        private readonly IFileNameSanitizer _fileNameSanitizer;

        public EpisodeClient(IResourceClient resourceClient, IRtmpDump rtmpDump, IFileNameSanitizer fileNameSanitizer)
        {
            if (resourceClient == null) throw new ArgumentNullException("resourceClient");
            if (rtmpDump == null) throw new ArgumentNullException("rtmpDump");
            if (fileNameSanitizer == null) throw new ArgumentNullException("fileNameSanitizer");

            _resourceClient = resourceClient;
            _rtmpDump = rtmpDump;
            _fileNameSanitizer = fileNameSanitizer;
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

            var link = resource.links.OrderByDescending(l => l.bitrateKbps).First();
            var rtmpUri = new Uri(link.uri);

            var fileName = _fileNameSanitizer.Sanitize(string.Format("{0}.flv", resource.postingTitle));

            _rtmpDump.Download(rtmpUri, fileName);
        }
    }
}