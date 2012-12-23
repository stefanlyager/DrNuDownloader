using System;
using System.ComponentModel;
using Args;

namespace DrNuDownloader.Console
{
    public class Arguments
    {
        [ArgsMemberSwitch("l")]
        [Description("List all episode URIs from this URI.")]
        public Uri ListUri { get; set; }

        [ArgsMemberSwitch("d")]
        [Description("Download episode from this URI.")]
        public Uri DownloadUri { get; set; }

        [ArgsMemberSwitch("da")]
        [Description("Download all episodes from this URI.")]
        public Uri DownloadAllUri { get; set; }
    }
}