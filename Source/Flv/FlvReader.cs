using System;
using System.IO;
using System.Linq;

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
            if (_header == null)
            {
                throw new InvalidOperationException("Header must be read before the first backpoint can be read.");
            }

            var bytes = _binaryReader.ReadBytes(4);
            if (bytes.Length != 4)
            {
                throw new InvalidOperationException(string.Format("Backpointer consists of 4 bytes, but only {0} bytes were read from Stream.", bytes.Length));
            }

            return new Backpointer(bytes);
        }

        public Tag ReadTag()
        {
            if (_header == null)
            {
                throw new InvalidOperationException("Header must be read before the first tag can be read.");
            }

            var bytes = _binaryReader.ReadBytes(4);
            if (bytes.Length != 4)
            {
                throw new InvalidOperationException(string.Format("Tag consists of at least 11 bytes, but only {0} bytes were read from Stream.", bytes.Length));
            }

            var payloadSize = new UInt24(bytes.Skip(1).Take(3), Endianness.BigEndian);

            var numberOfAdditionalBytesToRead = 7 + (int)payloadSize.Value;
            var additionalBytes = _binaryReader.ReadBytes(numberOfAdditionalBytesToRead);
            if (additionalBytes.Length != numberOfAdditionalBytesToRead)
            {
                throw new InvalidOperationException(string.Format("Tag consists of {0} bytes, but only {1} bytes were read from Stream.",
                                                                  bytes.Length + numberOfAdditionalBytesToRead,
                                                                  bytes.Length + additionalBytes.Length));
            }
            
            return new Tag(bytes.Concat(additionalBytes).ToArray());
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