using System;
using System.ComponentModel;
using Args;

namespace DrNuDownloader.Console
{
    [Description("Utility for downloading episodes from DR NU (http://www.dr.dk/tv).")]
    public class Arguments
    {
        [ArgsMemberSwitch("l")]
        [Description("List all episode URLs from this URL.")]
        public Uri ListUri { get; set; }

        [ArgsMemberSwitch("d")]
        [Description("Download episode from this URL.")]
        public Uri DownloadUri { get; set; }
    }
}