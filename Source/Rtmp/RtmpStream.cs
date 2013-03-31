using System;
using System.IO;
using System.Runtime.InteropServices;
using Rtmp.LibRtmp;
using Rtmp.LogStub;

namespace Rtmp
{
    public class RtmpStream : Stream
    {
        private readonly ILibRtmpWrapper _libRtmpWrapper;
        private readonly IUriData _uriData;
        private bool _canRead;
        private bool _isOpen;
        private long _position;
        private IntPtr _rtmp;
        private bool _durationFired;

        public override bool CanRead
        {
            get { return _canRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position {
            get { return _position; }
            set { throw new NotSupportedException(); }
        }

        public event EventHandler<DurationEventArgs> Duration;
        public event EventHandler<ElapsedEventArgs> Elapsed;

        public RtmpStream(ILibRtmpWrapper libRtmpWrapper, IUriData uriData)
        {
            if (libRtmpWrapper == null) throw new ArgumentNullException("libRtmpWrapper");
            if (uriData == null) throw new ArgumentNullException("uriData");

            _libRtmpWrapper = libRtmpWrapper;
            _uriData = uriData;
            _canRead = true;
            _isOpen = false;
            _position = 0;
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // TODO: Validate input parameters.

            if (!_isOpen)
            {
                Open();
            }

            int bytesRead;
            if (offset == 0 && buffer.Length == count)
            {
                // Read directly into buffer.
                bytesRead = _libRtmpWrapper.Read(_rtmp, buffer, buffer.Length);
            }
            else
            {
                // Create intermediary buffer.
                var intermediaryBuffer = new byte[count];
                bytesRead = _libRtmpWrapper.Read(_rtmp, intermediaryBuffer, intermediaryBuffer.Length);

                // Copy to buffer.
                for (int i = 0; i < bytesRead; i++)
                {
                    buffer[offset + i] = intermediaryBuffer[i];
                }
            }

            if (bytesRead == 0)
            {
                _canRead = false;
            }
            else
            {
                _position += bytesRead;    
            }

            if (!_durationFired)
            {
                var seconds = _libRtmpWrapper.GetDuration(_rtmp);
                var durationEventArgs = new DurationEventArgs(TimeSpan.FromSeconds(seconds));
                OnDuration(durationEventArgs);

                _durationFired = true;
            }

            var rtmp = (LibRtmp.Rtmp)Marshal.PtrToStructure(_rtmp, typeof(LibRtmp.Rtmp));
            if (rtmp.m_read.timestamp != 0)
            {
                var elapsed = TimeSpan.FromMilliseconds(rtmp.m_read.timestamp);
                OnElapsed(new ElapsedEventArgs(elapsed));
            }

            return bytesRead;
        }

        public void Open()
        {
            _libRtmpWrapper.LogSetLevel((int)LogLevel.All); // TODO: Investigate if flags can be used.
            var logCallback = new LogCallback((level, message) =>
            {
                switch (level)
                {
                    case LogLevel.Critical:
                    case LogLevel.Error:
                        throw new IOException(message);
                }
            });
            LogStubWrapper.SetLogCallback(logCallback);

            _rtmp = _libRtmpWrapper.Alloc();
            if (_rtmp == IntPtr.Zero)
            {
                throw new IOException("Unable to open RTMPStream.");
            }

            _libRtmpWrapper.Init(_rtmp);
            _libRtmpWrapper.SetupUrl(_rtmp, Marshal.StringToHGlobalAnsi(_uriData.ToUri()));

            LogStubWrapper.InitSockets();
            if (_libRtmpWrapper.Connect(_rtmp, IntPtr.Zero) == 0)
            {
                throw new IOException("Failed to establish RTMP connection.");
            }

            if (_libRtmpWrapper.ConnectStream(_rtmp, 0) == 0)
            {
                throw new IOException("Failed to establish RTMP session.");
            }

            _isOpen = true;
        }

        public override void Close()
        {
            base.Close();

            if (_rtmp != IntPtr.Zero)
            {
                _libRtmpWrapper.Close(_rtmp);
                _libRtmpWrapper.Free(_rtmp);
            }

            // TODO: Check that InitSockets() was called successfully.
            LogStubWrapper.CleanupSockets();

            _isOpen = false;
            _canRead = false;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
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