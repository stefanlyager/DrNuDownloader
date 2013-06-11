using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DrNuDownloader.Scrapers.Json.Converters
{
    public class AssetConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<IAsset>();

            var assets = JArray.Load(reader);
            foreach (var asset in assets)
            {
                if (asset["Kind"] != null && (string)asset["Kind"] == "VideoResource")
                {
                    var videoResourceAsset = new VideoResourceAsset();
                    serializer.Populate(asset.CreateReader(), videoResourceAsset);
                    result.Add(videoResourceAsset);
                }
            }

            return result.ToArray();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (IAsset);
        }
    }
}