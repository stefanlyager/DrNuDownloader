using System;
using System.Collections.Generic;
using DrNuDownloader.Clients;
using DrNuDownloader.Mappers;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Scrapers.Json;
using Rtmp;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        event EventHandler<DurationEventArgs> Duration;
        event EventHandler<ElapsedEventArgs> Elapsed;
        IEnumerable<Uri> GetEpisodeUris(Uri programUri);
        IProgram GetProgram(Uri programUri);
    }

    public class DrNuClient : IDrNuClient
    {
        private readonly IProgramClient _programClient;
        private readonly IEpisodeListClient _episodeListClient;
        private readonly IScraper<Uri> _programScraper;
        private readonly IScraper<Resource> _resourceScraper;
        private readonly IMapper<Resource, IProgram> _resourceMapper;

        public event EventHandler<DurationEventArgs> Duration;
        public event EventHandler<ElapsedEventArgs> Elapsed;

        public DrNuClient(IProgramClient programClient, IEpisodeListClient episodeListClient, IScraper<Uri> programScraper, IScraper<Resource> resourceScraper, IMapper<Resource, IProgram> resourceMapper)
        {
            if (programClient == null) throw new ArgumentNullException("programClient");
            if (episodeListClient == null) throw new ArgumentNullException("episodeListClient");
            if (programScraper == null) throw new ArgumentNullException("programScraper");
            if (resourceScraper == null) throw new ArgumentNullException("resourceScraper");
            if (resourceMapper == null) throw new ArgumentNullException("resourceMapper");

            _programClient = programClient;
            _episodeListClient = episodeListClient;
            _programScraper = programScraper;
            _resourceScraper = resourceScraper;
            _resourceMapper = resourceMapper;
        }

        public IProgram GetProgram(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var resourceUri = _programScraper.Scrape(programUri);
            var resource = _resourceScraper.Scrape(resourceUri);
            return _resourceMapper.Map(resource);
        }

        public IEnumerable<Uri> GetEpisodeUris(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var slug = _programClient.GetSlug(programUri);
            return _episodeListClient.GetEpisodeUris(slug);
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