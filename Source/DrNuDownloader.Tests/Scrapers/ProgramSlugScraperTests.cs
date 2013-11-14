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
    public class ProgramSlugScraperTests
    {
        private readonly Mock<IWebRequestWrapper> _webRequestWrapperMock;

        private readonly IProgramSlugScraper _programSlugScraper;

        public ProgramSlugScraperTests()
        {
            // Mock dependencies.
            _webRequestWrapperMock = new Mock<IWebRequestWrapper>();

            // System under test.
            _programSlugScraper = new ProgramSlugScraper(_webRequestWrapperMock.Object);
        }

        [Fact]
        public void Constructor_NullWebRequestWrapper_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new ProgramSlugScraper(null));
        }

        [Fact]
        public void Scrape_NullProgramUri_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _programSlugScraper.Scrape(null));
        }

        [Fact]
        public void Scrape_RelativeUri_ThrowsArgumentException()
        {
            // Arrange
            var relativeUri = new Uri("Relative URI", UriKind.Relative);

            // Act and assert.
            Assert.Throws<ArgumentException>(() => _programSlugScraper.Scrape(relativeUri));
        }

        [Fact]
        public void Scrape_ProgramSlugNotFound_ThrowsScraperException()
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
            Assert.Throws<ScraperException>(() => _programSlugScraper.Scrape(programUri));
        }

        [Fact]
        public void Scrape_ValidProgramUri_ReturnsSlug()
        {
            // Arrange
            var resource = Encoding.UTF8.GetBytes("programSerieSlug: \"slug\"");

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
            var slug = _programSlugScraper.Scrape(programUri);

            // Assert
            Assert.NotNull(slug);
            Assert.Equal("slug", slug);
        }
    }
}