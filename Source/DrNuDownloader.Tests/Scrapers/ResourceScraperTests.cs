using System;
using System.IO;
using System.Net;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Scrapers.Json;
using DrNuDownloader.Wrappers;
using Moq;
using Xunit;

namespace DrNuDownloader.Tests.Scrapers
{
    public class ResourceScraperTests
    {
        private readonly Mock<IWebRequestWrapper> _webRequestWrapperMock;
        private readonly Mock<IJsonConvertWrapper> _jsonConvertWrapperMock;

        private readonly IResourceScraper _resourceScraper;

        public ResourceScraperTests()
        {
            // Mock dependencies.
            _webRequestWrapperMock = new Mock<IWebRequestWrapper>();
            _jsonConvertWrapperMock = new Mock<IJsonConvertWrapper>();

            // System under test.
            _resourceScraper = new ResourceScraper(_webRequestWrapperMock.Object, _jsonConvertWrapperMock.Object);
        }

        [Fact]
        public void Constructor_NullWebRequestWrapper_ThrowsArgumentException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ResourceScraper(null, _jsonConvertWrapperMock.Object));
        }

        [Fact]
        public void Constructor_NullJsonConvertWrapper_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ResourceScraper(_webRequestWrapperMock.Object, null));
        }

        [Fact]
        public void Scrape_NullUri_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _resourceScraper.Scrape(null));
        }

        [Fact]
        public void Scrape_ValidUri_DeserializesJsonToObject()
        {
            // Arrange
            var resourceUri = new Uri("http://www.resource-uri.dk");

            var httpWebResponseMock = new Mock<HttpWebResponse>();
            httpWebResponseMock.Setup(hwr => hwr.GetResponseStream())
                               .Returns(new MemoryStream());

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(httpWebResponseMock.Object);

            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(resourceUri))
                                  .Returns(httpWebRequestMock.Object);

            // Act
            _resourceScraper.Scrape(resourceUri);

            // Assert
            _jsonConvertWrapperMock.Verify(jcw => jcw.DeserializeObject<Resource>(It.IsAny<string>()), Times.Once());
        }
    }
}