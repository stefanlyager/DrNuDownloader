using System;
using System.Runtime.Serialization;

namespace DrNuDownloader
{
    [Serializable]
    public class HtmlParseException : Exception
    {
        public HtmlParseException()
        {
        }

        public HtmlParseException(string message) : base(message)
        {
        }

        public HtmlParseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HtmlParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}