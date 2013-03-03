using System;
using System.Text;

namespace Rtmp
{
    public interface IUriData
    {
        Uri Uri { get; }
        bool IsLiveStream { get; }
        string TcUri { get; set; }
        string App { get; set; }
        string PlayPath { get; set; }
        string SwfUri { get; set; }
        bool? VerifySwf { get; set; }
        string FlashVersion { get; set; }
        TimeSpan? Buffer { get; set; }
        TimeSpan? Timeout { get; set; }
        string ToUri();
    }

    public class UriData : IUriData
    {
        public delegate IUriData Factory(Uri uri, bool isLiveStream);

        private readonly IKeyValuePairBuilder _keyValuePairBuilder;

        public Uri Uri { get; private set; }
        public bool IsLiveStream { get; private set; }

        public string TcUri { get; set; }
        public string App { get; set; }
        public string PlayPath { get; set; }

        public string SwfUri { get; set; }
        public bool? VerifySwf { get; set; }
        public string FlashVersion { get; set; }

        public TimeSpan? Buffer { get; set; }
        public TimeSpan? Timeout { get; set; }

        public UriData(IKeyValuePairBuilder keyValuePairBuilder, Uri uri, bool isLiveStream)
        {
            if (keyValuePairBuilder == null) throw new ArgumentNullException("keyValuePairBuilder");
            if (uri == null) throw new ArgumentNullException("uri");

            // TODO: Validate that uri is a valid RTMP URI.

            _keyValuePairBuilder = keyValuePairBuilder;
            Uri = uri;
            IsLiveStream = isLiveStream;
        }

        public string ToUri()
        {
            // TODO: Implement this with less redundant code.

            var stringBuilder = new StringBuilder(Uri.ToString());

            var keyValuePair = _keyValuePairBuilder.BuildKeyValuePair("live", IsLiveStream);
            stringBuilder.AppendFormat(" {0}", keyValuePair);

            AppendKeyValuePairToStringBuilder(stringBuilder, "tcUrl", TcUri);
            AppendKeyValuePairToStringBuilder(stringBuilder, "app", App);
            AppendKeyValuePairToStringBuilder(stringBuilder, "playpath", PlayPath);
            AppendKeyValuePairToStringBuilder(stringBuilder, "swfUrl", SwfUri);

            if (VerifySwf != null)
            {
                keyValuePair = _keyValuePairBuilder.BuildKeyValuePair("swfVfy", VerifySwf.Value);
                stringBuilder.AppendFormat(" {0}", keyValuePair);
            }

            AppendKeyValuePairToStringBuilder(stringBuilder, "flashVer", FlashVersion);

            if (Buffer != null)
            {
                keyValuePair = _keyValuePairBuilder.BuildKeyValuePair("buffer", Buffer.Value.Milliseconds);
                stringBuilder.AppendFormat(" {0}", keyValuePair);
            }

            if (Timeout != null)
            {
                keyValuePair = _keyValuePairBuilder.BuildKeyValuePair("timeout", Timeout.Value.Seconds);
                stringBuilder.AppendFormat(" {0}", keyValuePair);
            }

            return stringBuilder.ToString();
        }

        private void AppendKeyValuePairToStringBuilder(StringBuilder stringBuilder, string key, string value)
        {
            if (stringBuilder == null) throw new ArgumentNullException("stringBuilder");
            if (key == null) throw new ArgumentNullException("key");
            
            if (value == null) return;

            string keyValuePair = _keyValuePairBuilder.BuildKeyValuePair(key, value);
            stringBuilder.AppendFormat(" {0}", keyValuePair);
        }
    }
}