using System;
using System.IO;
using Flv;
using Rtmp;

namespace DrNuDownloader
{
    public interface IProgram
    {
        string Title { get; }
        string Genre { get; }
        string Description { get; }
        Uri RtmpUri { get; }

        event EventHandler<DurationEventArgs> Duration;
        event EventHandler<ElapsedEventArgs> Elapsed;

        void Download(string path);
    }

    public class Program : IProgram
    {
        private readonly IDrNuRtmpStreamFactory _drNuRtmpStreamFactory;
        private readonly IFileWrapper _fileWrapper;

        public string Title { get; private set; }
        public string Genre { get; private set; }
        public string Description { get; private set; }
        public Uri RtmpUri { get; private set; }

        public delegate IProgram Factory(string title, string genre, string description, Uri rtmpUri);

        public event EventHandler<DurationEventArgs> Duration;
        public event EventHandler<ElapsedEventArgs> Elapsed;

        public Program(IDrNuRtmpStreamFactory drNuRtmpStreamFactory, IFileWrapper fileWrapper, string title, string genre, string description, Uri rtmpUri)
        {
            if (drNuRtmpStreamFactory == null) throw new ArgumentNullException("drNuRtmpStreamFactory");
            if (fileWrapper == null) throw new ArgumentNullException("fileWrapper");
            if (title == null) throw new ArgumentNullException("title");
            if (genre == null) throw new ArgumentNullException("genre");
            if (description == null) throw new ArgumentNullException("description");
            if (rtmpUri == null) throw new ArgumentNullException("rtmpUri");

            _drNuRtmpStreamFactory = drNuRtmpStreamFactory;
            _fileWrapper = fileWrapper;
            Title = title;
            Genre = genre;
            Description = description;
            RtmpUri = rtmpUri;
        }

        public void Download(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            using (var drNuRtmpStream = _drNuRtmpStreamFactory.CreateDrNuRtmpStream(RtmpUri))
            {
                drNuRtmpStream.Duration += (sender, args) => OnDuration(args);
                drNuRtmpStream.Elapsed += (sender, args) => OnElapsed(args);

                using (var flvReader = new FlvReader(drNuRtmpStream))
                using (var flvWriter = new FlvWriter(_fileWrapper.Create(path)))
                {
                    IFlvPart flvPart;
                    while ((flvPart = flvReader.Read()) != null)
                    {
                        flvWriter.Write(flvPart);
                    }
                }
            }
        }

        protected virtual void OnDuration(DurationEventArgs durationEventArgs)
        {
            if (durationEventArgs == null) throw new ArgumentNullException("durationEventArgs");

            var handler = Duration;
            if (handler != null)
            {
                handler(this, durationEventArgs);
            }
        }

        protected virtual void OnElapsed(ElapsedEventArgs elapsedEventArgs)
        {
            if (elapsedEventArgs == null) throw new ArgumentNullException("elapsedEventArgs");

            var handler = Elapsed;
            if (handler != null)
            {
                handler(this, elapsedEventArgs);
            }
        }
    }
}