using System;

namespace Rtmp
{
    public class DurationEventArgs : EventArgs
    {
        public TimeSpan Duration { get; private set; }

        public DurationEventArgs(TimeSpan duration)
        {
            Duration = duration;
        }
    }
}