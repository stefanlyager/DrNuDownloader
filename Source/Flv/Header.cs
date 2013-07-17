using System;
using System.Linq;
using System.Text;

namespace Flv
{
    public class Header : IFlvPart
    {
        public byte[] Bytes { get; private set; }

        public Header(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length != 9) throw new ArgumentException("FLV header should always consist of exactly 9 bytes.", "bytes");

            Bytes = bytes;
        }

        public string GetSignature()
        {
            return Encoding.ASCII.GetString(Bytes.Take(3).ToArray());
        }

        public byte GetVersion()
        {
            return Bytes[3];
        }

        public byte GetFlags() // TODO: Should have an enum with Flags attribute as return type.
        {
            return Bytes[4];
        }

        public uint GetHeaderSize()
        {
            return BitConverter.ToUInt32(Bytes.Skip(5).Take(4).Reverse().ToArray(), 0);
        }

        public byte[] ToByteArray()
        {
            return Bytes;
        }
    }
}