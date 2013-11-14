using System;
using System.IO;
using System.Net;
using System.Text;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Wrappers;
using Moq;
using Xunit;

namespace DrNuDownloader.Tests.Scrapers
{
    public class ResourceUriScraperTests
    {
        private readonly Mock<IWebRequestWrapper> _webRequestWrapperMock;

        private readonly IResourceUriScraper _resourceUriScraper;

        public ResourceUriScraperTests()
        {
            // Mock dependencies.
            _webRequestWrapperMock = new Mock<IWebRequestWrapper>();

            // System under test.
            _resourceUriScraper = new ResourceUriScraper(_webRequestWrapperMock.Object);
        }

        [Fact]
        public void Constructor_NullWebRequestWrapper_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ResourceUriScraper(null));
        }

        [Fact]
        public void Scrape_NullProgramUri_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _resourceUriScraper.Scrape(null));
        }

        [Fact]
        public void Scrape_RelativeUri_ThrowsArgumentException()
        {
            // Arrange
            var relativeUri = new Uri("Relative URI", UriKind.Relative);

            // Act and assert.
            Assert.Throws<ArgumentException>(() => _resourceUriScraper.Scrape(relativeUri));
        }

        [Fact]
        public void Scrape_WebExceptionIsThrown_ThrowsScraperException()
        {
            // Arrange
            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Throws<WebException>();
            
            var programUri = new Uri("http://www.program-uri.dk");
            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(programUri))
                                  .Returns(httpWebRequestMock.Object);

            // Act and assert
            Assert.Throws<ScraperException>(() => _resourceUriScraper.Scrape(programUri));
        }

        [Fact]
        public void Scrape_ResourceUriNotFound_ThrowsScraperException()
        {
            // Arrange
            var webResponseMock = new Mock<WebResponse>();
            webResponseMock.Setup(wr => wr.GetResponseStream())
                           .Returns(new MemoryStream());

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(webResponseMock.Object);

            var programUri = new Uri("http://www.program-uri.dk");
            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(programUri))
                                  .Returns(httpWebRequestMock.Object);

            // Act and assert.
            Assert.Throws<ScraperException>(() => _resourceUriScraper.Scrape(programUri));
        }

        [Fact]
        public void Scrape_ValidProgramUri_ReturnsResourceUri()
        {
            // Arrange
            var resource = Encoding.UTF8.GetBytes("resource: \"http://www.resource-uri.dk\"");

            var webResponseMock = new Mock<WebResponse>();
            webResponseMock.Setup(wr => wr.GetResponseStream())
                           .Returns(new MemoryStream(resource));

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(webResponseMock.Object);

            var programUri = new Uri("http://www.program-uri.dk");
            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(programUri))
                                  .Returns(httpWebRequestMock.Object);

            // Act
            var resourceUri = _resourceUriScraper.Scrape(programUri);

            // Assert
            Assert.NotNull(resourceUri);
            Assert.Equal(new Uri("http://www.resource-uri.dk"), resourceUri);
        }
    }
}