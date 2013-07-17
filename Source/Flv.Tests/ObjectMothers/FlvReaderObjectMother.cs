using System.IO;
using System.Linq;

namespace Flv.Tests.ObjectMothers
{
    public class FlvReaderObjectMother
    {
        public FlvReader CreateEmptyFlvReader()
        {
            return CreateFlvReader(new byte[0]);
        }

        public FlvReader CreateFlvReaderWithHeader()
        {
            var flvReader = CreateFlvReader(new byte[9]);
            flvReader.MoveToBeforeHeaderState();

            return flvReader;
        }
        
        public FlvReader CreateFlvReaderWithBackpointer()
        {
            var flvReader = CreateFlvReader(new byte[4]);
            flvReader.MoveToBeforeBackpointerState();

            return flvReader;
        }

        public FlvReader CreateFlvReaderWithTag()
        {
            var bytes = Enumerable.Repeat((byte)0, 18).ToList();
            var payloadSize = new UInt24(10);
            bytes.InsertRange(1, payloadSize.ToByteArray(Endianness.BigEndian));

            var flvReader = CreateFlvReader(bytes.ToArray());
            flvReader.MoveToBeforeTagState();

            return flvReader;
        }

        private FlvReader CreateFlvReader(byte[] bytes)
        {
            var memoryStream = new MemoryStream(bytes);
            return new FlvReader(memoryStream);
        }
    }
}