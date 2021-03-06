﻿using System;
using Moq;
using Xunit;

namespace DrNuDownloader.Tests
{
    public class ProgramTests
    {
        private readonly Mock<IDrNuRtmpStreamFactory> _drNuRtmpStreamFactoryMock;
        private readonly Mock<IFileWrapper> _fileWrapper;

        private readonly Program _program;

        public ProgramTests()
        {
            // Mock dependencies.
            _drNuRtmpStreamFactoryMock = new Mock<IDrNuRtmpStreamFactory>();
            _fileWrapper = new Mock<IFileWrapper>();

            // System under test.
            _program = new Program(_drNuRtmpStreamFactoryMock.Object, _fileWrapper.Object, "Title", "Genre", "Description", new Uri("http://www.stefanlyager.dk"));
        }

        [Fact]
        public void Constructor_NullDrNuStreamFactory_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(null, _fileWrapper.Object, "Title", "Genre", "Description", new Uri("http://www.stefanlyager.dk")); });
        }

        [Fact]
        public void Constructor_NullFileWrapper_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(_drNuRtmpStreamFactoryMock.Object, null, "Title", "Genre", "Description", new Uri("http://www.stefanlyager.dk")); });
        }

        [Fact]
        public void Constructor_NullTitle_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(_drNuRtmpStreamFactoryMock.Object, _fileWrapper.Object, null, "Genre", "Description", new Uri("http://www.stefanlyager.dk")); });
        }

        [Fact]
        public void Constructor_NullGenre_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(_drNuRtmpStreamFactoryMock.Object, _fileWrapper.Object, "Title", null, "Description", new Uri("http://www.stefanlyager.dk")); });
        }

        [Fact]
        public void Constructor_NullDescription_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(_drNuRtmpStreamFactoryMock.Object, _fileWrapper.Object, "Title", "Genre", null, new Uri("http://www.stefanlyager.dk")); });
        }

        [Fact]
        public void Constructor_NullUri_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => { new Program(_drNuRtmpStreamFactoryMock.Object, _fileWrapper.Object, "Title", "Genre", "Description", null); });
        }

        [Fact]
        public void Download_NullPath_ThrowsArgumentNullException()
        {
            // Act and assert.
            Assert.Throws<ArgumentNullException>(() => _program.Download(null));
        }
    }
}