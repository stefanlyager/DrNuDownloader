using System;
using System.Linq;

namespace Flv
{
    public class Backpointer : IFlvPart
    {
        public byte[] Bytes { get; private set; }

        public Backpointer(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length != 4) throw new ArgumentException("Backpointer should consist of exactly 4 bytes.", "bytes");

            Bytes = bytes;
        }

        public uint GetValue()
        {
            return BitConverter.ToUInt32(Bytes.Take(4).Reverse().ToArray(), 0);
        }

        public byte[] ToByteArray()
        {
            return Bytes;
        }
    }
}