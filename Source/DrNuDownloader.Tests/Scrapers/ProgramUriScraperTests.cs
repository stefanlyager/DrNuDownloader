using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DrNuDownloader.Scrapers;
using DrNuDownloader.Wrappers;
using Moq;
using Xunit;

namespace DrNuDownloader.Tests.Scrapers
{
    public class ProgramUriScraperTests
    {
        private readonly Mock<IWebRequestWrapper> _webRequestWrapperMock;

        private readonly IProgramUriScraper _programUriScraper;

        public ProgramUriScraperTests()
        {
            // Mock dependencies.
            _webRequestWrapperMock = new Mock<IWebRequestWrapper>();

            // System under test.
            _programUriScraper = new ProgramUriScraper(_webRequestWrapperMock.Object);
        }

        [Fact]
        public void Constructor_NullWebRequestWrapper_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => new ProgramUriScraper(null));
        }

        [Fact]
        public void Scrape_NullSlug_ThrowsArgumentNullException()
        {
            // Act and assert
            Assert.Throws<ArgumentNullException>(() => _programUriScraper.Scrape(null));
        }

        [Fact]
        public void Scrape_NoLiElementsFound_ThrowsScraperException()
        {
            // Arrange
            var webResponseMock = new Mock<WebResponse>();
            webResponseMock.Setup(wr => wr.GetResponseStream())
                           .Returns(new MemoryStream());

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(webResponseMock.Object);

            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(It.IsAny<Uri>()))
                                  .Returns(httpWebRequestMock.Object);

            Slug slug = "slug";

            // Act and assert.
            Assert.Throws<ScraperException>(() => _programUriScraper.Scrape(slug));
        }

        [Fact]
        public void Scrape_NoUrisFound_ThrowsScraperException()
        {
            // Arrange
            var html = Encoding.UTF8.GetBytes("<li>Program #1</li>\r\n" +
                                              "<li>Program #2</li>\r\n" +
                                              "<li>Program #3</li>");

            var webResponseMock = new Mock<WebResponse>();
            webResponseMock.Setup(wr => wr.GetResponseStream())
                           .Returns(new MemoryStream(html));

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(webResponseMock.Object);

            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(It.IsAny<Uri>()))
                                  .Returns(httpWebRequestMock.Object);

            Slug slug = "slug";

            // Act
            Assert.Throws<ScraperException>(() => _programUriScraper.Scrape(slug));
        }

        [Fact]
        public void Scrape_ValidSlug_ReturnsProgramUris()
        {
            // Arrange
            var html = Encoding.UTF8.GetBytes("<li><a href=\"/programUri1\">Program #1</a></li>\r\n" +
                                              "<li><a href=\"/programUri2\">Program #2</a></li>\r\n" +
                                              "<li><a href=\"/programUri3\">Program #3</a></li>");

            var webResponseMock = new Mock<WebResponse>();
            webResponseMock.Setup(wr => wr.GetResponseStream())
                           .Returns(new MemoryStream(html));

            var httpWebRequestMock = new Mock<HttpWebRequest>();
            httpWebRequestMock.Setup(hwr => hwr.GetResponse())
                              .Returns(webResponseMock.Object);

            _webRequestWrapperMock.Setup(wrw => wrw.CreateHttp(It.IsAny<Uri>()))
                                  .Returns(httpWebRequestMock.Object);

            Slug slug = "slug";

            // Act
            var programUris = _programUriScraper.Scrape(slug);

            // Assert
            Assert.NotNull(programUris);

            var programUrisList = programUris.ToList();
            Assert.Equal(3, programUrisList.Count);
            Assert.True(programUrisList[0].ToString().EndsWith("programUri1"));
            Assert.True(programUrisList[1].ToString().EndsWith("programUri2"));
            Assert.True(programUrisList[2].ToString().EndsWith("programUri3"));
        }
    }
}