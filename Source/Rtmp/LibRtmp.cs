using System;
using System.Runtime.InteropServices;

namespace Rtmp
{
    internal class LibRtmp
    {
        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_LibVersion();

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr RTMP_Alloc();

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RTMP_Free(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RTMP_Init(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RTMP_Close(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RTMP_EnableWrite(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_SetupURL(IntPtr rtmp, IntPtr url);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RTMP_LogSetLevel(int logLevel);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_Connect(IntPtr rtmp, IntPtr cp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_ConnectStream(IntPtr rtmp, int seekTime);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_Read(IntPtr rtmp, byte[] buffer, int size);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RTMP_Pause(IntPtr rtmp, int doPause);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RTMP_IsConnected(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RTMP_IsTimedout(IntPtr rtmp);
    }
}