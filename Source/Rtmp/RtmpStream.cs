using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Rtmp
{
    public class RtmpStream : Stream
    {
        private readonly IUriData _uriData;
        private bool _canRead;
        private bool _isOpen;
        private long _position;
        private IntPtr _rtmp;

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

        public RtmpStream(IUriData uriData)
        {
            if (uriData == null) throw new ArgumentNullException("uriData");

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
                bytesRead = LibRtmp.RTMP_Read(_rtmp, buffer, buffer.Length);
            }
            else
            {
                // Create intermediary buffer.
                var intermediaryBuffer = new byte[count];
                bytesRead = LibRtmp.RTMP_Read(_rtmp, intermediaryBuffer, intermediaryBuffer.Length);

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

            return bytesRead;
        }

        private void Open()
        {
            LibRtmp.RTMP_LogSetLevel((int)LogLevel.All); // TODO: Investigate if flags can be used.
            var logCallback = new LogStub.LogCallback((level, message) =>
            {
                switch (level)
                {
                    case LogLevel.Critical:
                    case LogLevel.Error:
                        throw new IOException(message);
                }
            });
            LogStub.SetLogCallback(logCallback);

            _rtmp = LibRtmp.RTMP_Alloc();
            if (_rtmp == IntPtr.Zero)
            {
                throw new IOException("Unable to open RTMPStream.");
            }

            LibRtmp.RTMP_Init(_rtmp);
            LibRtmp.RTMP_SetupURL(_rtmp, Marshal.StringToHGlobalAnsi(_uriData.ToUri()));

            LogStub.InitSockets();
            if (LibRtmp.RTMP_Connect(_rtmp, IntPtr.Zero) == 0)
            {
                throw new IOException("Failed to establish RTMP connection.");
            }

            if (LibRtmp.RTMP_ConnectStream(_rtmp, 0) == 0)
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
                LibRtmp.RTMP_Close(_rtmp);
                LibRtmp.RTMP_Free(_rtmp);
            }

            // TODO: Check that InitSockets() was called successfully.
            LogStub.CleanupSockets();

            _isOpen = false;
            _canRead = false;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}