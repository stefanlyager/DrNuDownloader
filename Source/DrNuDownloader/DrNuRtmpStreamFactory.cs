using System;
using System.Text.RegularExpressions;
using DrNuDownloader.Clients;
using Rtmp;
using Rtmp.LibRtmp;

namespace DrNuDownloader
{
    public interface IDrNuRtmpStreamFactory
    {
        IRtmpStream CreateDrNuRtmpStream(Uri rtmpUri);
    }

    public class DrNuRtmpStreamFactory : IDrNuRtmpStreamFactory
    {
        private readonly ILibRtmpWrapper _libRtmpWrapper;
        private readonly UriData.Factory _uriDataFactory;

        public DrNuRtmpStreamFactory(ILibRtmpWrapper libRtmpWrapper, UriData.Factory uriDataFactory)
        {
            if (libRtmpWrapper == null) throw new ArgumentNullException("libRtmpWrapper");
            if (uriDataFactory == null) throw new ArgumentNullException("uriDataFactory");

            _libRtmpWrapper = libRtmpWrapper;
            _uriDataFactory = uriDataFactory;
        }

        public IRtmpStream CreateDrNuRtmpStream(Uri rtmpUri)
        {
            if (rtmpUri == null) throw new ArgumentNullException("rtmpUri");

            // TODO: Some of the values below should be parsed from rtmpUri while others should be extracted to app.config.
            IUriData uriData = _uriDataFactory(rtmpUri, false);
            uriData.App = "cms";                         // TODO: App should be parsed from rtmpUri.
            uriData.TcUri = "rtmp://vod.dr.dk:1935/cms"; // TODO: TcUri should be parsed from rtmpUri.
            uriData.SwfUri = "http://www.dr.dk/NU/assets/swf/NetTVPlayer.swf";
            uriData.FlashVersion = "LNX 10,1,102,64";
            uriData.VerifySwf = true;

            var regex = new Regex("(?<playpath>mp4:CMS.*)");
            var match = regex.Match(rtmpUri.AbsoluteUri);
            if (!match.Success)
            {
                throw new ParseException("Error parsing RTMP URI.");
            }
            uriData.PlayPath = match.Groups["playpath"].Value;

            uriData.Buffer = TimeSpan.FromMilliseconds(36000000);
            uriData.Timeout = TimeSpan.FromSeconds(30);

            return new RtmpStream(_libRtmpWrapper, uriData);
        }
    }
}