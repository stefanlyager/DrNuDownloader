using System;
using System.Collections.Generic;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        IEnumerable<Episode> GetEpisodes(Uri programUri);
    }

    public class DrNuClient : IDrNuClient
    {
        private readonly IProgramClient _programClient;
        private readonly IEpisodeListClient _episodeListClient;

        public DrNuClient(IProgramClient programClient, IEpisodeListClient episodeListClient)
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