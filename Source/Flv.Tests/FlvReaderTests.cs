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
            // System under test.
            _flvReader = CreateFlvReader(9);
        }

        private IFlvReader CreateFlvReader(int streamLength)
        {
            var memoryStream = new MemoryStream(new byte[streamLength]);
            return new FlvReader(memoryStream);
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

        [Fact]
        public void ReadBackpointer_HeaderNotReadYet_ThrowsInvalidOperationException()
        {
            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_ReachedEndOfStream_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader.ReadHeader();

            // Act
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_AfterHeaderIsRead_ReturnsBackpointer()
        {
            // Arrange
            _flvReader = CreateFlvReader(13);
            _flvReader.ReadHeader();

            // Act
            var backpointer = _flvReader.ReadBackpointer();

            // Assert
            Assert.NotNull(backpointer);
        }
    }
}