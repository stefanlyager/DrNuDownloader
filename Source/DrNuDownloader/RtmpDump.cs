using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cmd;

namespace DrNuDownloader
{
    public interface IRtmpDump
    {
        void Download(Uri rtmpUri, string fileName);
    }

    public class RtmpDump : IRtmpDump
    {
        public void Download(Uri rtmpUri, string fileName)
        {
            if (rtmpUri == null) throw new ArgumentNullException("rtmpUri");

            var regex = new Regex("(?<playpath>mp4:CMS.*)");
            var match = regex.Match(rtmpUri.AbsoluteUri);
            if (!match.Success)
            {
                throw new ParseException("Error parsing RTMP URI.");
            }

            string playPath = match.Groups["playpath"].Value;

            dynamic cmd = new Cmd();
            cmd.rtmpdump(r: FormatArgument("rtmp://vod.dr.dk/cms"),
                         a: FormatArgument("cms"),
                         f: FormatArgument("LNX 10,1,102,64"),
                         W: FormatArgument("http://www.dr.dk/NU/assets/swf/NetTVPlayer.swf"),
                         y: FormatArgument(playPath),
                         o: FormatArgument(fileName));
        }

        private string FormatArgument(string argument)
        {
            if (argument == null) throw new ArgumentNullException("argument");

            return string.Format("\"{0}\"", argument);
        }
    }
}