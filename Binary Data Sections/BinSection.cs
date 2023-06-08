using System.IO;
using System.Text;

namespace BWPrimitivesReader.BinaryDataSections
{
    public abstract class BinSection
    {
        protected string _format = string.Empty;
        public string Format => _format;

        protected void ReadFormat(BinaryReader binaryReader)
        {
            byte[] formatBytes = binaryReader.ReadBytes(64);

            int stringEndPos = 0;

            for (int i = 0; i < formatBytes.Length; i++)
            {
                if (formatBytes[i] == 0x00) { stringEndPos = i; break; }
            }

            _format = Encoding.UTF8.GetString(formatBytes, 0, stringEndPos);
        }
    }
}