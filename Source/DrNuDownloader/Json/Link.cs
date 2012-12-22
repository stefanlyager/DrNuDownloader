namespace DrNuDownloader.Json
{
    public class Link
    {
        public string qualityId { get; set; }
        public string uri { get; set; }
        public string linkType { get; set; }
        public string fileType { get; set; }
        public int bitrateKbps { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}