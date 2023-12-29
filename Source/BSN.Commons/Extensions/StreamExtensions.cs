using System.IO;

namespace BSN.Commons.Extensions
{
    public static class StreamExtensions
    {
        public static MemoryStream ToMemoryStream(this Stream input)
        {
            byte[] inputBuffer = new byte[input.Length];

            input.Read(inputBuffer, 0, inputBuffer.Length);

            return new MemoryStream(inputBuffer);
        }
    }
}
