using System;
using System.Collections.Generic;
using DrNuDownloader.Clients;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        IEnumerable<Uri> GetEpisodeUris(Uri programUri);
        void Download(Uri episodeUri);
    }

    public class DrNuClient : IDrNuClient
    {
        private readonly IProgramClient _programClient;
        private readonly IEpisodeListClient _episodeListClient;
        private readonly IEpisodeClient _episodeClient;

        public DrNuClient(IProgramClient programClient, IEpisodeListClient episodeListClient, IEpisodeClient episodeClient)
        {
            if (programClient == null) throw new ArgumentNullException("programClient");
            if (episodeListClient == null) throw new ArgumentNullException("episodeListClient");
            if (episodeClient == null) throw new ArgumentNullException("episodeClient");

            _programClient = programClient;
            _episodeListClient = episodeListClient;
            _episodeClient = episodeClient;
        }

        public IEnumerable<Uri> GetEpisodeUris(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var slug = _programClient.GetSlug(programUri);
            return _episodeListClient.GetEpisodeUris(slug);
        }

        public void Download(Uri episodeUri)
        {
            if (episodeUri == null) throw new ArgumentNullException("episodeUri");

            _episodeClient.Download(episodeUri);
        }
    }
}