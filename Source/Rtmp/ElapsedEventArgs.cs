using System;

namespace Rtmp
{
    public class ElapsedEventArgs : EventArgs
    {
        public TimeSpan Elapsed { get; private set; }
        public long Bytes { get; set; }

        public ElapsedEventArgs(TimeSpan duration, long bytes)
        {
            Elapsed = duration;
            Bytes = bytes;
        }
    }
}