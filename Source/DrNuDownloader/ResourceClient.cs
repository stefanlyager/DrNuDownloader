using System;
using System.IO;
using System.Linq;
using System.Net;
using DrNuDownloader.Json;
using Newtonsoft.Json;

namespace DrNuDownloader
{
    public interface IResourceClient
    {
        Uri GetRtmpUri(Uri resourceUri);
    }

    public class ResourceClient : IResourceClient
    {
        public Uri GetRtmpUri(Uri resourceUri)
        {
            if (resourceUri == null) throw new ArgumentNullException("resourceUri");

            var request = WebRequest.CreateHttp(resourceUri);
            var response = request.GetResponse();

            String json;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            var resource = JsonConvert.DeserializeObject<Resource>(json);
            var link = resource.links.OrderByDescending(l => l.bitrateKbps).First();
            return new Uri(link.uri);
        }
    }
}