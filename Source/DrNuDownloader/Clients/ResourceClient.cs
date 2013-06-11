using System;
using System.IO;
using System.Net;
using DrNuDownloader.Scrapers.Json;
using Newtonsoft.Json;

namespace DrNuDownloader.Clients
{
    public interface IResourceClient
    {
        Resource GetResource(Uri resourceUri);
    }

    public class ResourceClient : IResourceClient
    {
        public Resource GetResource(Uri resourceUri)
        {
            if (resourceUri == null) throw new ArgumentNullException("resourceUri");

            var request = WebRequest.CreateHttp(resourceUri);
            var response = request.GetResponse();

            string json;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<Resource>(json);
        }
    }
}