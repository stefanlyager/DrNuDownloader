using System;
using Flv.Tests.ObjectMothers;
using Xunit;

namespace Flv.Tests
{
    public class FlvReaderTests : IDisposable
    {
        private FlvReader _flvReader;
        private readonly FlvReaderObjectMother _flvReaderObjectMother;

        public FlvReaderTests()
        {
            _flvReaderObjectMother = new FlvReaderObjectMother();
        }

        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => new FlvReader(null));
        }

        [Fact]
        public void ReadHeader_ReachedEndOfStream_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeHeader_ReturnsHeader()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act
            var header = _flvReader.ReadHeader();

            // Assert
            Assert.NotNull(header);
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeBackpointer_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeTag_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadHeader());
        }

        [Fact]
        public void ReadBackpointer_ReachedEndOfStream_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();
            _flvReader.MoveToBeforeBackpointerState();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeBackpointer_ReturnsBackpointer()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act
            var backpointer = _flvReader.ReadBackpointer();

            // Assert
            Assert.NotNull(backpointer);
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeTag_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadTag_ReachedEndOfStream_ReturnsNull()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();
            _flvReader.MoveToBeforeTagState();

            // Act
            var tag = _flvReader.ReadTag();

            // Assert
            Assert.Null(tag);
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadTag());
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeBackpointer_ThrowsInvalidOperationException()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => _flvReader.ReadTag());
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeTag_ReturnsTag()
        {
            // Arrange
            _flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act
            var tag = _flvReader.ReadTag();

            // Assert
            Assert.NotNull(tag);
        }

        public void Dispose()
        {
            if (_flvReader != null)
            {
                _flvReader.Dispose();
            }
        }
    }
}