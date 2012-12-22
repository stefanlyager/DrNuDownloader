using System;
using System.Collections.Generic;

namespace DrNuDownloader
{
    public class DrNuClient
    {
        private readonly ProgramClient _programClient;
        private readonly EpisodeListClient _episodeListClient;

        public DrNuClient(ProgramClient programClient, EpisodeListClient episodeListClient)
        {
            if (programClient == null) throw new ArgumentNullException("programClient");
            if (episodeListClient == null) throw new ArgumentNullException("episodeListClient");
            _programClient = programClient;
            _episodeListClient = episodeListClient;
        }

        public IEnumerable<Episode> GetEpisodes(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var programId = _programClient.GetId(programUri);
            return _episodeListClient.GetEpisodes(programId);
        }
    }
}