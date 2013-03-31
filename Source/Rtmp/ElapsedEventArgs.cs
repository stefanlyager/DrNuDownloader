using System;

namespace Rtmp
{
    public class ElapsedEventArgs : EventArgs
    {
        public TimeSpan Elapsed { get; private set; }

        public ElapsedEventArgs(TimeSpan duration)
        {
            Elapsed = duration;
        }
    }
}