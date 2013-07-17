using System;
using System.IO;

namespace Flv
{
    public interface IFlvWriter : IDisposable
    {
        void Write<T>(T flvPart) where T : IFlvPart;
    }

    public class FlvWriter : IFlvWriter
    {
        private readonly BinaryWriter _binaryWriter;

        public FlvWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _binaryWriter = new BinaryWriter(stream);
        }

        public void Write<T>(T flvPart) where T : IFlvPart
        {
            if (flvPart == null) throw new ArgumentNullException("flvPart");

            _binaryWriter.Write(flvPart.ToByteArray());
        }

        public void Dispose()
        {
            if (_binaryWriter != null)
            {
                _binaryWriter.Dispose();
            }
        }
    }
}