using System;

namespace Flv
{
    public interface IFlvReaderState
    {
        bool CanReadHeader { get; }
        bool CanReadBackpointer { get; }
        bool CanReadTag { get; }
        IFlvPart Read();
        Header ReadHeader();
        Backpointer ReadBackpointer();
        Tag ReadTag();
    }

    public abstract class FlvReaderState : IFlvReaderState
    {
        protected internal FlvReader FlvReader { get; private set; }

        public abstract bool CanReadHeader { get; }
        public abstract bool CanReadBackpointer { get; }
        public abstract bool CanReadTag { get; }

        protected FlvReaderState(FlvReader flvReader)
        {
            if (flvReader == null) throw new ArgumentNullException("flvReader");

            FlvReader = flvReader;
        }

        public abstract IFlvPart Read();
        public abstract Header ReadHeader();
        public abstract Backpointer ReadBackpointer();
        public abstract Tag ReadTag();
    }
}