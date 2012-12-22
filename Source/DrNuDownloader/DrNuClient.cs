using System;
using System.Collections.Generic;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        IEnumerable<Episode> GetEpisodes(Uri programUri);
        void Download(Episode episode);
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

        public IEnumerable<Episode> GetEpisodes(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var programId = _programClient.GetId(programUri);
            return _episodeListClient.GetEpisodes(programId);
        }

        public void Download(Uri episodeUri)
        {
            if (episodeUri == null) throw new ArgumentNullException("episodeUri");

            _episodeClient.Download(episodeUri);
        }

        public void Download(Episode episode)
        {
            if (episode == null) throw new ArgumentNullException("episode");

            _episodeClient.Download(episode);
        }
    }
}