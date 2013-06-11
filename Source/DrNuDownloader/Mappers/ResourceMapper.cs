using System;
using System.Linq;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Scrapers.Json;

namespace DrNuDownloader.Mappers
{
    public class ResourceMapper : IMapper<Resource, IProgram>
    {
        private readonly Program.Factory _programFactory;

        public ResourceMapper(Program.Factory programFactory)
        {
            if (programFactory == null) throw new ArgumentNullException("programFactory");

            _programFactory = programFactory;
        }

        public IProgram Map(Resource resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");

            var data = GetData(resource);
            var videoResourceAsset = GetVideoResourceAsset(data);
            var link = GetLink(videoResourceAsset);

            return _programFactory(data.Title ?? string.Empty,
                                   data.OnlineGenreText ?? string.Empty,
                                   data.Description ?? string.Empty,
                                   new Uri(link.Uri));
        }

        private Data GetData(Resource resource)
        {
            if (resource.Data == null)
            {
                throw new ScraperException("Property 'Data' not found.");
            }

            var data = resource.Data.FirstOrDefault();
            if (data == null)
            {
                throw new ScraperException("No elements found in 'Data' array.");
            }

            return data;
        }

        private VideoResourceAsset GetVideoResourceAsset(Data data)
        {
            if (data.Assets == null)
            {
                throw new ScraperException("Property 'Assets' not found.");
            }

            var videoResourceAsset = data.Assets.FirstOrDefault() as VideoResourceAsset;
            if (videoResourceAsset == null)
            {
                throw new ScraperException("No element with property 'Kind' set to 'VideoResource' was found in 'Assets' array .");
            }

            return videoResourceAsset;
        }
        
        private Link GetLink(VideoResourceAsset videoResourceAsset)
        {
            if (videoResourceAsset.Links == null)
            {
                throw new ScraperException("Property 'Links' not found.");
            }

            var link = videoResourceAsset.Links.Where(l => l.Target == "Streaming")
                                               .OrderByDescending(l => l.Bitrate).FirstOrDefault();
            if (link == null)
            {
                throw new ScraperException("No element with property 'Target' set to 'Streaming' was found in 'Links' array.");
            }

            return link;
        }
    }
}