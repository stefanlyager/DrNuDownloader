using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RtmpSockBuf
    {
        public int sb_socket;
        public int sb_size;		/* number of unprocessed bytes in buffer */

        [MarshalAs(UnmanagedType.LPStr)]
        public string sb_start;		/* pointer into sb_pBuffer of next byte to process */

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16384)]
        public string sb_buf; /* data read from socket */

        public int sb_timedout;
        public IntPtr sb_ssl;
    }
}