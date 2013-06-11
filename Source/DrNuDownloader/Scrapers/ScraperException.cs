using System;
using System.Runtime.Serialization;

namespace DrNuDownloader.Scrapers
{
    [Serializable]
    public class ScraperException : Exception
    {
        public ScraperException()
        {
        }

        public ScraperException(string message) : base(message)
        {
        }

        public ScraperException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ScraperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}