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
        private IFlvReaderState _flvReaderState;

        public FlvReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _binaryReader = new BinaryReader(stream);
            MoveToBeforeHeaderState();
        }

        internal void MoveToBeforeHeaderState()
        {
            _flvReaderState = new BeforeHeaderState(this);
        }

        internal void MoveToBeforeBackpointerState()
        {
            _flvReaderState = new BeforeBackpointerState(this);
        }

        internal void MoveToBeforeTagState()
        {
            _flvReaderState = new BeforeTagState(this);
        }

        public Header ReadHeader()
        {
            return _flvReaderState.ReadHeader();
        }

        public Backpointer ReadBackpointer()
        {
            return _flvReaderState.ReadBackpointer();
        }

        public Tag ReadTag()
        {
            return _flvReaderState.ReadTag();
        }

        public void Dispose()
        {
            if (_binaryReader != null)
            {
                _binaryReader.Dispose();
            }
        }

        private class BeforeHeaderState : FlvReaderState
        {
            public BeforeHeaderState(FlvReader flvReader) : base(flvReader)
            {
            }

            public override Header ReadHeader()
            {
                var bytes = FlvReader._binaryReader.ReadBytes(9);
                if (bytes.Length != 9)
                {
                    throw new InvalidOperationException(string.Format("FLV header consists of 9 bytes, but only {0} bytes were read from Stream.", bytes.Length));
                }

                FlvReader.MoveToBeforeBackpointerState();
                return new Header(bytes);
            }

            public override Backpointer ReadBackpointer()
            {
                throw new InvalidOperationException("Header must be read before reading backpointer.");
            }

            public override Tag ReadTag()
            {
                throw new InvalidOperationException("Header and the first backpointer must be read before reading tag.");
            }
        }

        private class BeforeTagState : FlvReaderState
        {
            public BeforeTagState(FlvReader flvReader) : base(flvReader)
            {
            }

            public override Header ReadHeader()
            {
                throw new InvalidOperationException("Header has already been read.");
            }

            public override Backpointer ReadBackpointer()
            {
                throw new InvalidOperationException("Tag must be read before reading backpointer.");
            }

            public override Tag ReadTag()
            {
                var bytes = FlvReader._binaryReader.ReadBytes(4);
                if (bytes.Length == 0)
                {
                    return null;
                }

                if (bytes.Length != 4)
                {
                    throw new InvalidOperationException(string.Format("Tag consists of at least 11 bytes, but only {0} bytes were read from Stream.", bytes.Length));
                }

                var payloadSize = new UInt24(bytes.Skip(1).Take(3), Endianness.BigEndian);

                var numberOfAdditionalBytesToRead = 7 + (int)payloadSize.Value;
                var additionalBytes = FlvReader._binaryReader.ReadBytes(numberOfAdditionalBytesToRead);
                if (additionalBytes.Length != numberOfAdditionalBytesToRead)
                {
                    throw new InvalidOperationException(string.Format("Tag consists of {0} bytes, but only {1} bytes were read from Stream.",
                                                                      bytes.Length + numberOfAdditionalBytesToRead,
                                                                      bytes.Length + additionalBytes.Length));
                }

                FlvReader.MoveToBeforeBackpointerState();
                return new Tag(bytes.Concat(additionalBytes).ToArray());
            }
        }

        private class BeforeBackpointerState : FlvReaderState
        {
            public BeforeBackpointerState(FlvReader flvReader) : base(flvReader)
            {
            }

            public override Header ReadHeader()
            {
                throw new InvalidOperationException("Header has already been read.");
            }

            public override Backpointer ReadBackpointer()
            {
                var bytes = FlvReader._binaryReader.ReadBytes(4);
                if (bytes.Length != 4)
                {
                    throw new InvalidOperationException(string.Format("Backpointer consists of 4 bytes, but only {0} bytes were read from Stream.", bytes.Length));
                }

                FlvReader.MoveToBeforeTagState();
                return new Backpointer(bytes);
            }

            public override Tag ReadTag()
            {
                throw new InvalidOperationException("Backpointer must be read before reading tag.");
            }
        }
    }
}