using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RtmpLnk
    {
        public AVal hostname;
        public AVal sockshost;

        public AVal playpath0;	/* parsed from URL */
        public AVal playpath;	/* passed in explicitly */
        public AVal tcUrl;
        public AVal swfUrl;
        public AVal pageUrl;
        public AVal app;
        public AVal auth;
        public AVal flashVer;
        public AVal subscribepath;
        public AVal usherToken;
        public AVal token;
        public AVal pubUser;
        public AVal pubPasswd;
        public AmfObject extras;
        public int edepth;

        public int seekTime;
        public int stopTime;

        public int lFlags;

        public int swfAge;

        public int protocol;
        public int timeout;		/* connection timeout in seconds */

        public int pFlags;

        public ushort socksport;
        public ushort port;

        public IntPtr dh;			/* for encryption */
        public IntPtr rc4keyIn;
        public IntPtr rc4keyOut;

        public uint SWFSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U1)]
        public byte[] SWFHash;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 42)]
        public string SWFVerificationResponse;
    }
}