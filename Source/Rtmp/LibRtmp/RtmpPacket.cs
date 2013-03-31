using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RtmpPacket
    {
        public byte m_headerType;
        public byte m_packetType;
        public byte m_hasAbsTimestamp;	/* timestamp absolute or relative? */
        public int m_nChannel;
        public uint m_nTimeStamp;	/* timestamp */
        public int m_nInfoField2;	/* last 4 bytes in a long header */
        public uint m_nBodySize;
        public uint m_nBytesRead;
        public IntPtr m_chunk;

        [MarshalAs(UnmanagedType.LPStr)]
        public string m_body;
    }
}