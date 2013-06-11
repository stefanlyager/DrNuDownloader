using System;
using DrNuDownloader.Mappers;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Scrapers.Json;
using Moq;
using Xunit;

namespace DrNuDownloader.Tests.Mappers
{
    public class ResourceMapperTests
    {
        private readonly Mock<IProgram> _programMock;

        private readonly IMapper<Resource, IProgram> _resourceMapper;

        public ResourceMapperTests()
        {
            // Mock dependencies.
            _programMock = new Mock<IProgram>();

            // System under test.
            _resourceMapper = new ResourceMapper((title, genre, description, rtmpUri) => _programMock.Object);
        }

        [Fact]
        public void Constructor_NullProgramFactory_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ResourceMapper(null));
        }

        [Fact]
        public void Map_NullResource_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _resourceMapper.Map(null));
        }

        [Fact]
        public void Map_NullDataProperty_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource();

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_EmptyDataArray_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource { Data = new Data[0] };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_NullAssetsProperty_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data() }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_EmptyAssetsArray_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new IAsset[0] } }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_AssetsArrayWithoutVideoResourceAsset_ThrowsScraperException()
        {
            // Arrange
            var assetMock = new Mock<IAsset>();
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new[] { assetMock.Object } } }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_NullLinksProperty_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new IAsset[] { new VideoResourceAsset() } } }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_EmptyLinksArray_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new IAsset[] { new VideoResourceAsset { Links = new Link[0] } } } }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_LinksArrayWithoutStreamingLink_ThrowsScraperException()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new IAsset[] { new VideoResourceAsset { Links = new[] { new Link() } } } } }
                               };

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceMapper.Map(resource));
        }

        [Fact]
        public void Map_ValidResource_CallsProgramFactory()
        {
            // Arrange
            var resource = new Resource
                               {
                                   Data = new[] { new Data { Assets = new IAsset[] { new VideoResourceAsset { Links = new[] { new Link { Target = "Streaming", Uri = "http://www.rtmp-uri.dk" } } } } } }
                               };

            // Act
            var program = _resourceMapper.Map(resource);

            // Assert
            Assert.NotNull(program);
        }
    }
}