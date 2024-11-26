using System.IO;
using System.IO.Compression;

namespace Factorio_Headless_Server_Tool
{
    public class FileProcessor
    {
        public byte[] UnpackZLib(string file)
        {
            var data = File.ReadAllBytes(file);
            byte[] result;
            using (var input = new MemoryStream(data))
            using (var decompressor = new ZLibStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                decompressor.CopyTo(output);
                result = output.ToArray();
            }
            return result;
        }

        public byte[] PackZLib(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var compressor = new ZLibStream(output, CompressionLevel.Optimal))
                {
                    compressor.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }
    }
}
