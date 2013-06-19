using System;
using System.ComponentModel;
using Args;

namespace DrNuDownloader.Console
{
    [Description("Utility for downloading videos from DR NU (http://www.dr.dk/tv).")]
    public class Arguments
    {
        [ArgsMemberSwitch("l")]
        [Description("List URLs of all videos found at this URL.")]
        public Uri ListUri { get; set; }

        [ArgsMemberSwitch("d")]
        [Description("Download episode from this URL.")]
        public Uri DownloadUri { get; set; }
    }
}