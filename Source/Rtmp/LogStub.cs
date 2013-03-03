using System.Runtime.InteropServices;

namespace Rtmp
{
    internal class LogStub
    {
        public delegate void LogCallback(LogLevel logLevel, string message);

        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitSockets();

        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CleanupSockets();

        [DllImport("logstub.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLogCallback(LogCallback logCallback);
    }
}