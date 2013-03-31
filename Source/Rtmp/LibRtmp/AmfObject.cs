using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AmfObject
    {
        public int o_num;
        public IntPtr o_props;
    }
}