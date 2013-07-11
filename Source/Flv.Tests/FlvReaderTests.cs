using System;
using System.IO;
using Xunit;

namespace Flv.Tests
{
    public class FlvReaderTests
    {
        private IFlvReader _flvReader;

        public FlvReaderTests()
        {
            // Create dependency.
            var memoryStream = new MemoryStream(new byte[9]);

            // System under test.
            _flvReader = new FlvReader(memoryStream);
        }

        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new FlvReader(null));
        }

        [Fact]
        public void ReadHeader_LessThanNineBytesInStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var memoryStream = new MemoryStream(new byte[8]);
            _flvReader = new FlvReader(memoryStream);

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_HeaderAlreadyRead_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader.ReadHeader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_HeaderNotReadYet_ReturnsHeader()
        {
            // Act
            var header = _flvReader.ReadHeader();
            
            // Assert
            Assert.NotNull(header);
        }
    }
}