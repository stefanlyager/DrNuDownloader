using System;
using System.IO;

namespace Flv
{
    public interface IFlvReader : IDisposable
    {
        Header ReadHeader();
        Backpointer ReadBackpointer();
        Tag ReadTag();
    }

    public class FlvReader : IFlvReader
    {
        private readonly BinaryReader _binaryReader;
        private Header _header;

        public FlvReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _binaryReader = new BinaryReader(stream);
        }

        public Header ReadHeader()
        {
            if (_header != null)
            {
                throw new InvalidOperationException("Header has already been read.");
            }

            var bytes = _binaryReader.ReadBytes(9);
            if (bytes.Length != 9)
            {
                throw new InvalidOperationException(string.Format("FLV header consists of 9 bytes, but only {0} bytes were read from Stream.", bytes.Length));
            }

            return _header = new Header(bytes);
        }

        public Backpointer ReadBackpointer()
        {
            throw new NotImplementedException();
        }

        public Tag ReadTag()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_binaryReader != null)
            {
                _binaryReader.Dispose();
            }
        }
    }
}