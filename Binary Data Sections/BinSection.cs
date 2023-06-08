using System.IO;
using System.Text;

namespace BWPrimitivesReader.BinaryDataSections
{
    public abstract class BinSection
    {
        public string Format { get; private set; } = string.Empty;

        protected void ReadFormat(BinaryReader binaryReader)
        {
            byte[] formatBytes = binaryReader.ReadBytes(64);

            int stringEndPos = 0;

            for (int i = 0; i < formatBytes.Length; i++)
            {
                if (formatBytes[i] == 0x00) { stringEndPos = i; break; }
            }

            Format = Encoding.UTF8.GetString(formatBytes, 0, stringEndPos);
        }
    }
}