using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    public interface ILibRtmpWrapper
    {
        int LibVersion();
        IntPtr Alloc();
        void Free(IntPtr rtmp);
        void Init(IntPtr rtmp);
        void Close(IntPtr rtmp);
        void EnableWrite(IntPtr rtmp);
        int SetupUrl(IntPtr rtmp, IntPtr url);
        void LogSetLevel(int logLevel);
        int Connect(IntPtr rtmp, IntPtr cp);
        int ConnectStream(IntPtr rtmp, int seekTime);
        int Read(IntPtr rtmp, byte[] buffer, int size);
        int Pause(IntPtr rtmp, int doPause);
        bool IsConnected(IntPtr rtmp);
        bool IsTimedout(IntPtr rtmp);
        double GetDuration(IntPtr rtmp);
    }

    internal class LibRtmpWrapper : ILibRtmpWrapper
    {
        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_LibVersion();

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RTMP_Alloc();

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RTMP_Free(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RTMP_Init(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RTMP_Close(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RTMP_EnableWrite(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_SetupURL(IntPtr rtmp, IntPtr url);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RTMP_LogSetLevel(int logLevel);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_Connect(IntPtr rtmp, IntPtr cp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_ConnectStream(IntPtr rtmp, int seekTime);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_Read(IntPtr rtmp, byte[] buffer, int size);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int RTMP_Pause(IntPtr rtmp, int doPause);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool RTMP_IsConnected(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool RTMP_IsTimedout(IntPtr rtmp);

        [DllImport("librtmp.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double RTMP_GetDuration(IntPtr rtmp);

        public int LibVersion()
        {
            return RTMP_LibVersion();
        }

        public IntPtr Alloc()
        {
            return RTMP_Alloc();
        }

        public void Free(IntPtr rtmp)
        {
            RTMP_Free(rtmp);
        }

        public void Init(IntPtr rtmp)
        {
            RTMP_Init(rtmp);
        }

        public void Close(IntPtr rtmp)
        {
            RTMP_Close(rtmp);
        }

        public void EnableWrite(IntPtr rtmp)
        {
            RTMP_EnableWrite(rtmp);
        }

        public int SetupUrl(IntPtr rtmp, IntPtr url)
        {
            return RTMP_SetupURL(rtmp, url);
        }

        public void LogSetLevel(int logLevel)
        {
            RTMP_LogSetLevel(logLevel);
        }

        public int Connect(IntPtr rtmp, IntPtr cp)
        {
            return RTMP_Connect(rtmp, cp);
        }

        public int ConnectStream(IntPtr rtmp, int seekTime)
        {
            return RTMP_ConnectStream(rtmp, seekTime);
        }

        public int Read(IntPtr rtmp, byte[] buffer, int size)
        {
            return RTMP_Read(rtmp, buffer, size);
        }

        public int Pause(IntPtr rtmp, int doPause)
        {
            return RTMP_Pause(rtmp, doPause);
        }

        public bool IsConnected(IntPtr rtmp)
        {
            return RTMP_IsConnected(rtmp);
        }

        public bool IsTimedout(IntPtr rtmp)
        {
            return RTMP_IsTimedout(rtmp);
        }

        public double GetDuration(IntPtr rtmp)
        {
            return RTMP_GetDuration(rtmp);
        }
    }
}