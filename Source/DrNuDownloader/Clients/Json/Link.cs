using Newtonsoft.Json;

namespace DrNuDownloader.Clients.Json
{
    public class Link
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("bitrateKbps")]
        public int BitrateInKbps { get; set; }
    }
}