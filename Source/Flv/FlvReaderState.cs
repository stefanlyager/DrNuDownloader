using System;

namespace Flv
{
    public interface IFlvReaderState
    {
        Header ReadHeader();
        Backpointer ReadBackpointer();
        Tag ReadTag();
    }

    public abstract class FlvReaderState : IFlvReaderState
    {
        protected internal FlvReader FlvReader { get; private set; }

        protected FlvReaderState(FlvReader flvReader)
        {
            if (flvReader == null) throw new ArgumentNullException("flvReader");

            FlvReader = flvReader;
        }

        public abstract Header ReadHeader();
        public abstract Backpointer ReadBackpointer();
        public abstract Tag ReadTag();
    }
}