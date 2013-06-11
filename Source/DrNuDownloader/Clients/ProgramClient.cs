using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using DrNuDownloader.Scrapers;

namespace DrNuDownloader.Clients
{
    public interface IProgramClient
    {
        string GetSlug(Uri programUri);
    }

    public class ProgramClient : IProgramClient
    {
        public string GetSlug(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var request = WebRequest.CreateHttp(programUri);
            var response = request.GetResponse();

            var streamReader = new StreamReader(response.GetResponseStream());
            var html = streamReader.ReadToEnd();

            var regex = new Regex("programSerieSlug:\\s*\"(?<slug>.*)\"");
            var match = regex.Match(html);
            if (!match.Success)
            {
                throw new ScraperException("Unable to find programSerieSlug.");
            }

            return match.Groups["slug"].Value;
        }
    }
}