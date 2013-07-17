using System;
using System.Collections.Generic;
using System.Linq;

namespace Flv
{
    public struct UInt24
    {
        private readonly byte[] _bytes;

        public static uint MaxValue
        {
            get
            {
                return (2 ^ 24) - 1;
            }
        }

        public uint Value
        {
            get
            {
                return _bytes[0] | ((uint)_bytes[1] << 8) | ((uint)_bytes[2] << 16);
            }
        }

        public UInt24(IEnumerable<byte> bytes, Endianness endianness)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");

            var bytesArray = bytes.ToArray();
            if (bytesArray.Length != 3) throw new ArgumentException("UInt24 should consist of exactly 3 bytes.", "bytes");

            _bytes = ConvertConditional(bytesArray, endianness);
        }

        public UInt24(byte[] bytes, Endianness endianness)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length != 3) throw new ArgumentException("UInt24 should consist of exactly 3 bytes.", "bytes");

            _bytes = ConvertConditional(bytes, endianness);
        }

        public UInt24(uint value)
        {
            if (value > MaxValue) throw new ArgumentOutOfRangeException("value", string.Format("UInt24 must be less than or equal {0}.", MaxValue));

            _bytes = BitConverter.IsLittleEndian ? BitConverter.GetBytes(value).Take(3).ToArray() :
                                                   BitConverter.GetBytes(value).Skip(1).Take(3).Reverse().ToArray();
        }

        public byte[] ToByteArray(Endianness endianness)
        {
            return ConvertConditional(_bytes, endianness);
        }

        private static byte[] ConvertConditional(byte[] bytes, Endianness endianness)
        {
            return endianness == Endianness.LittleEndian ? bytes :
                                                           bytes.Reverse().ToArray();
        }
    }
} 