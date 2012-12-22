using System;
using System.Collections.Generic;

namespace DrNuDownloader
{
    public interface IEpisodeListClient
    {
        IEnumerable<Episode> GetEpisodes(string programId);
    }

    public class EpisodeListClient : IEpisodeListClient
    {
        public IEnumerable<Episode> GetEpisodes(string programId)
        {
            throw new NotImplementedException();
        }
    }
}