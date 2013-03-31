using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AVal
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string av_val;
        public int av_len;
    }
}