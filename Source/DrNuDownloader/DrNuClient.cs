using System;
using System.Collections.Generic;
using DrNuDownloader.Mappers;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Scrapers.Json;

namespace DrNuDownloader
{
    public interface IDrNuClient
    {
        IEnumerable<Uri> GetProgramUris(Uri programUri);
        IProgram GetProgram(Uri programUri);
    }

    public class DrNuClient : IDrNuClient
    {
        private readonly IResourceUriScraper _resourceUriScraper;
        private readonly IProgramUriScraper _programUriScraper;
        private readonly IScraper<Uri, Resource> _resourceScraper;
        private readonly IMapper<Resource, IProgram> _resourceMapper;
        private readonly IProgramSlugScraper _programSlugScraper;

        public DrNuClient(IResourceUriScraper resourceUriScraper, IResourceScraper resourceScraper, IResourceMapper resourceMapper, IProgramSlugScraper programSlugScraper, IProgramUriScraper programUriScraper)
        {
            if (resourceUriScraper == null) throw new ArgumentNullException("resourceUriScraper");
            if (resourceScraper == null) throw new ArgumentNullException("resourceScraper");
            if (resourceMapper == null) throw new ArgumentNullException("resourceMapper");
            if (programSlugScraper == null) throw new ArgumentNullException("programSlugScraper");
            if (programUriScraper == null) throw new ArgumentNullException("programUriScraper");

            _resourceUriScraper = resourceUriScraper;
            _programUriScraper = programUriScraper;
            _resourceScraper = resourceScraper;
            _resourceMapper = resourceMapper;
            _programSlugScraper = programSlugScraper;
        }

        public IProgram GetProgram(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var resourceUri = _resourceUriScraper.Scrape(programUri);
            var resource = _resourceScraper.Scrape(resourceUri);
            return _resourceMapper.Map(resource);
        }

        public IEnumerable<Uri> GetProgramUris(Uri programUri)
        {
            if (programUri == null) throw new ArgumentNullException("programUri");

            var slug = _programSlugScraper.Scrape(programUri);
            return _programUriScraper.Scrape(slug);
        }
    }
}