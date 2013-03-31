using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RtmpRead
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string buf;

        [MarshalAs(UnmanagedType.LPStr)]
        public string bufpos;

        public uint buflen;
        public uint timestamp;
        public byte dataType;
        public byte flags;
        public byte status;
        public byte initialFrameType;
        public uint nResumeTS;

        [MarshalAs(UnmanagedType.LPStr)]
        public string metaHeader;

        [MarshalAs(UnmanagedType.LPStr)]
        public string initialFrame;

        public uint nMetaHeaderSize;
        public uint nInitialFrameSize;
        public uint nIgnoredFrameCounter;
        public uint nIgnoredFlvFrameCounter;
    }
}