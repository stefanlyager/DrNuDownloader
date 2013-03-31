using System;
using System.Collections.Generic;
using DrNuDownloader.Clients;
using Rtmp;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        event EventHandler<DurationEventArgs> Duration;
        event EventHandler<ElapsedEventArgs> Elapsed;
        IEnumerable<Uri> GetEpisodeUris(Uri programUri);
        void Download(Uri episodeUri);
    }

    public class DrNuClient : IDrNuClient
    {
        private readonly IProgramClient _programClient;
        private readonly IEpisodeListClient _episodeListClient;
        private readonly IEpisodeClient _episodeClient;

        public event EventHandler<DurationEventArgs> Duration;
        public event EventHandler<ElapsedEventArgs> Elapsed;

        public DrNuClient(IProgramClient programClient, IEpisodeListClient episodeListClient, IEpisodeClient episodeClient)
        {
            if (programClient == null) throw new ArgumentNullException("programClient");
            if (episodeListClient == null) throw new ArgumentNullException("episodeListClient");
            if (episodeClient == null) throw new ArgumentNullException("episodeClient");

            _programClient = programClient;
            _episodeListClient = episodeListClient;
            _episodeClient = episodeClient;

            _episodeClient.Duration += (sender, args) => OnDuration(args);
            _episodeClient.Elapsed += (sender, args) => OnElapsed(args);
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

        protected virtual void OnDuration(DurationEventArgs durationEventArgs)
        {
            if (durationEventArgs == null) throw new ArgumentNullException("durationEventArgs");

            var handler = Duration;
            if (handler != null)
            {
                handler(this, durationEventArgs);
            }
        }

        protected virtual void OnElapsed(ElapsedEventArgs elapsedEventArgs)
        {
            if (elapsedEventArgs == null) throw new ArgumentNullException("elapsedEventArgs");

            var handler = Elapsed;
            if (handler != null)
            {
                handler(this, elapsedEventArgs);
            }
        }
    }
}