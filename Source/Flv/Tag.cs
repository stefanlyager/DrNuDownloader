using System;
using System.Linq;

namespace Flv
{
    public class Tag : IFlvPart
    {
        public byte[] Bytes { get; private set; }

        public Tag(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length <= 11) throw new ArgumentException("Tag should consist of more than 11 bytes.", "bytes");

            Bytes = bytes;
        }

        public byte GetPacketType()
        {
            return Bytes[0];
        }

        public UInt24 GetPayloadSize()
        {
            return new UInt24(Bytes.Skip(1).Take(3), Endianness.BigEndian);
        }

        public UInt24 GetLowerTimestamp()
        {
            return new UInt24(Bytes.Skip(4).Take(3), Endianness.BigEndian);
        }

        public byte GetUpperTimestamp()
        {
            return Bytes[7];
        }

        public UInt24 GetStreamId()
        {
            return new UInt24(Bytes.Skip(8).Take(3), Endianness.BigEndian);
        }

        public byte[] GetPayload()
        {
            return Bytes.Skip(11).ToArray();
        }

        public byte[] ToByteArray()
        {
            return Bytes;
        }
    }
}