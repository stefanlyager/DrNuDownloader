using Newtonsoft.Json;

namespace DrNuDownloader.Clients.Json
{
    public class Resource
    {
        [JsonProperty("postingTitle")]
        public string PostingTitle { get; set; }

        [JsonProperty("links")]
        public Link[] Links { get; set; }
    }
}