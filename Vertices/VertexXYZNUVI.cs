using System;
using System.IO;

namespace BWPrimitivesReader.Vertices
{
    [Serializable]
    public class VertexXYZNUVI : VertexXYZNUV
    {
        private readonly byte _i1;
        public byte I1 => _i1;

        private byte[] _padding;

        public VertexXYZNUVI(BinaryReader binaryReader) : base(binaryReader)
        {
            _i1 = binaryReader.ReadByte();

            _padding = binaryReader.ReadBytes(3);
        }
    }
}