using System;
using System.ComponentModel;
using Args;

namespace DrNuDownloader.Console
{
    [Description("Værktøj til at downloade videoer fra DR NU (http://www.dr.dk/tv).")]
    public class Arguments
    {
        [ArgsMemberSwitch("l")]
        [Description("Viser URL'er på alle videoer, som blev fundet på denne URL.")]
        public Uri ListUri { get; set; }

        [ArgsMemberSwitch("d")]
        [Description("Downloader video fra denne URL.")]
        public Uri DownloadUri { get; set; }
    }
}