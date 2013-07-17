using System;
using Flv.Tests.ObjectMothers;
using Xunit;

namespace Flv.Tests
{
    public class FlvReaderTests
    {
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
            var flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeHeader_ReturnsHeader()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act
            var header = flvReader.ReadHeader();

            // Assert
            Assert.NotNull(header);
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeBackpointer_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadHeader());
        }

        [Fact]
        public void ReadHeader_WhenStateIsBeforeTag_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadHeader());
        }

        [Fact]
        public void ReadBackpointer_ReachedEndOfStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();
            flvReader.MoveToBeforeBackpointerState();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeBackpointer_ReturnsBackpointer()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act
            var backpointer = flvReader.ReadBackpointer();

            // Assert
            Assert.NotNull(backpointer);
        }

        [Fact]
        public void ReadBackpointer_WhenStateIsBeforeTag_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadBackpointer());
        }

        [Fact]
        public void ReadTag_ReachedEndOfStream_ReturnsNull()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateEmptyFlvReader();
            flvReader.MoveToBeforeTagState();

            // Act
            var tag = flvReader.ReadTag();

            // Assert
            Assert.Null(tag);
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeHeader_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithHeader();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadTag());
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeBackpointer_ThrowsInvalidOperationException()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithBackpointer();

            // Act and assert.
            Assert.Throws<InvalidOperationException>(() => flvReader.ReadTag());
        }

        [Fact]
        public void ReadTag_WhenStateIsBeforeTag_ReturnsTag()
        {
            // Arrange
            var flvReader = _flvReaderObjectMother.CreateFlvReaderWithTag();

            // Act
            var tag = flvReader.ReadTag();

            // Assert
            Assert.NotNull(tag);
        }
    }
}