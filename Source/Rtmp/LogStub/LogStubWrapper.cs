using System.Runtime.InteropServices;

namespace Rtmp.LogStub
{
    internal class LogStubWrapper
    {
        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitSockets();

        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CleanupSockets();

        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLogCallback(LogCallback logCallback);
    }
}