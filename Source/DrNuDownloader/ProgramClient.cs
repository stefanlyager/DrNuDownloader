using System;
using System.Net;
using HtmlAgilityPack;

namespace DrNuDownloader
{
    public interface IProgramClient
    {
        string GetId(Uri programUri);
    }

    public class ProgramClient : IProgramClient
    {
        public string GetId(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var request = WebRequest.CreateHttp(programUri);
            var response = request.GetResponse();

            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(response.GetResponseStream());

            var articleElement = htmlDocument.DocumentNode.SelectSingleNode("//article[@class='programSerieSpotContainer']");
            return articleElement.Attributes["id"].Value;
        }
    }
}