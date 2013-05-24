using DrNuDownloader.Clients.Json.Converters;
using Newtonsoft.Json;

namespace DrNuDownloader.Clients.Json
{
    public class Data
    {
        public string Title { get; set; }

        [JsonConverter(typeof(AssetConverter))]
        public IAsset[] Assets { get; set; }
    }
}