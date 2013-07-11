using System;
using System.IO;

namespace Flv
{
    public class FlvWriter
    {
        private readonly Stream _stream;

        public FlvWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _stream = stream;
        }

        public void WriteHeader(Header header)
        {
            throw new NotImplementedException();
        }

        public void WriteBackpointer(Backpointer backpointer)
        {
            throw new NotImplementedException();
        }

        public void WriteTag(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}