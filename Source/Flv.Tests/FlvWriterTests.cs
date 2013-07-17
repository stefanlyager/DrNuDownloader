using System;
using System.IO;
using Xunit;

namespace Flv.Tests
{
    public class FlvWriterTests : IDisposable
    {
        private readonly MemoryStream _memoryStream;
        private readonly IFlvWriter _flvWriter;

        public FlvWriterTests()
        {
            // Create dependency.
            _memoryStream = new MemoryStream();

            // System under test.
            _flvWriter = new FlvWriter(_memoryStream);
        }

        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new FlvWriter(null));
        }

        [Fact]
        public void Write_NullFlvPart_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _flvWriter.Write((IFlvPart)null));
        }

        [Fact]
        public void Write_ValidArguments_BytesAreWritten()
        {
            // Arrange
            var bytes = new byte[4];
            var backpointer = new Backpointer(bytes);

            // Act
            _flvWriter.Write(backpointer);

            // Assert
            Assert.Equal(4, _memoryStream.Length);
        }

        public void Dispose()
        {
            if (_flvWriter != null)
            {
                _flvWriter.Dispose();
            }
        }
    }
}