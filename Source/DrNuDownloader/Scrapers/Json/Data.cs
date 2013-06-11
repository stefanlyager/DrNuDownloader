using DrNuDownloader.Scrapers.Json.Converters;
using Newtonsoft.Json;

namespace DrNuDownloader.Scrapers.Json
{
    public class Data
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OnlineGenreText { get; set; }

        [JsonConverter(typeof(AssetConverter))]
        public IAsset[] Assets { get; set; }
    }
}