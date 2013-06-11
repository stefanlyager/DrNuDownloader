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
    public class ProgramScraperTests
    {
        private readonly Mock<IWebRequestWrapper> _webRequestWrapperMock;

        private readonly IScraper<Uri> _programScraper;

        public ProgramScraperTests()
        {
            // Mock dependencies.
            _webRequestWrapperMock = new Mock<IWebRequestWrapper>();

            // System under test.
            _programScraper = new ProgramScraper(_webRequestWrapperMock.Object);
        }

        [Fact]
        public void Constructor_NullWebRequestWrapper_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ProgramScraper(null));
        }

        [Fact]
        public void Scrape_NullProgramUri_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _programScraper.Scrape(null));
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
            Assert.Throws<ScraperException>(() => _programScraper.Scrape(programUri));
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
            var resourceUri = _programScraper.Scrape(programUri);

            // Assert
            Assert.NotNull(resourceUri);
            Assert.Equal(new Uri("http://www.resource-uri.dk"), resourceUri);
        }
    }
}