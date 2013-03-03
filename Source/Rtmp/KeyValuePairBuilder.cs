using System;
using System.Globalization;

namespace Rtmp
{
    public interface IKeyValuePairBuilder
    {
        string BuildKeyValuePair(string key, string value);
        string BuildKeyValuePair(string key, bool value);
        string BuildKeyValuePair(string key, int value);
        string BuildKeyValuePair(string key, long value);
    }

    public class KeyValuePairBuilder : IKeyValuePairBuilder
    {
        public string BuildKeyValuePair(string key, string value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            return string.Format("{0}={1}", key, Encode(value));
        }

        public string BuildKeyValuePair(string key, bool value)
        {
            if (key == null) throw new ArgumentNullException("key");

            return BuildKeyValuePair(key, value ? "1" : "0");
        }

        public string BuildKeyValuePair(string key, int value)
        {
            if (key == null) throw new ArgumentNullException("key");

            return BuildKeyValuePair(key, (long)value);
        }

        public string BuildKeyValuePair(string key, long value)
        {
            if (key == null) throw new ArgumentNullException("key");

            return BuildKeyValuePair(key, value.ToString(CultureInfo.InvariantCulture));
        }

        private string Encode(string valueToEncode)
        {
            if (valueToEncode == null) throw new ArgumentNullException("valueToEncode");

            // TODO: Figure out which additional special characters should be encoded.
            return valueToEncode.Replace(" ", @"\20").Replace(@"\", @"\5c");
        }
    }
}