using Newtonsoft.Json;

namespace DrNuDownloader.Wrappers
{
    public interface IJsonConvertWrapper
    {
        T DeserializeObject<T>(string json);
    }

    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}