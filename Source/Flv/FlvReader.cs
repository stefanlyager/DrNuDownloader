using System;
using System.IO;

namespace Flv
{
    public class FlvReader
    {
        private readonly Stream _stream;

        public FlvReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _stream = stream;
        }

        public Header ReadHeader()
        {
            throw new NotImplementedException();
        }

        public Backpointer ReadBackpointer()
        {
            throw new NotImplementedException();
        }

        public Tag ReadTag()
        {
            throw new NotImplementedException();
        }
    }
}