using System.IO;
using System.Numerics;

namespace BWPrimitivesReader
{
    public static class BinaryReaderExtensionMethods
    {
        public static Vector3 ReadVector3(this BinaryReader binaryReader)
        {
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();
            return new Vector3(x, y, z);
        }

        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            return new Vector2(x, y);
        }
    }
}