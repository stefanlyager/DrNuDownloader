using System;
using System.Runtime.InteropServices;

namespace Rtmp.LibRtmp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rtmp
    {
        public int m_inChunkSize;
        public int m_outChunkSize;
        public int m_nBWCheckCounter;
        public int m_nBytesIn;
        public int m_nBytesInSent;
        public int m_nBufferMS;
        public int m_stream_id;		/* returned in _result from createStream */
        public int m_mediaChannel;
        public uint m_mediaStamp;
        public uint m_pauseStamp;
        public int m_pausing;
        public int m_nServerBW;
        public int m_nClientBW;
        public byte m_nClientBW2;
        public byte m_bPlaying;
        public byte m_bSendEncoding;
        public byte m_bSendCounter;

        public int m_numInvokes;
        public int m_numCalls;
        public IntPtr m_methodCalls; //RTMP_METHOD* m_methodCalls;	/* remote method calls queue */

        public int m_channelsAllocatedIn;
        public int m_channelsAllocatedOut;
        public IntPtr m_vecChannelsIn; //RTMPPacket** m_vecChannelsIn;
        public IntPtr m_vecChannelsOut; //RTMPPacket** m_vecChannelsOut;
        public IntPtr m_channelTimestamp; //int* m_channelTimestamp;	/* abs timestamp of last packet */

        public double m_fAudioCodecs;	/* audioCodecs for the connect packet */
        public double m_fVideoCodecs;	/* videoCodecs for the connect packet */
        public double m_fEncoding;		/* AMF0 or AMF3 */

        public double m_fDuration;		/* duration of stream in seconds */

        public int m_msgCounter;		/* RTMPT stuff */
        public int m_polling;
        public int m_resplen;
        public int m_unackd;
        public AVal m_clientID;

        public RtmpRead m_read;
        public RtmpPacket m_write;
        public RtmpSockBuf m_sb;
        public RtmpLnk Link;
    }
}