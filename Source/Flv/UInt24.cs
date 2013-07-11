using System;

namespace Flv
{
    public struct UInt24
    {
        private readonly byte[] _bytes;

        public uint Value
        {
            get
            {
                return _bytes[0] | ((uint)_bytes[1] << 8) | ((uint)_bytes[2] << 16);
            }
        }

        public UInt24(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length != 3) throw new ArgumentException("UInt24 should consist of exactly 3 bytes.", "bytes");

            _bytes = bytes;
        }
    }
}