using System;
using System.IO;

namespace BWPrimitivesReader.Vertices
{
    [Serializable]
    public class VertexXYZNUVIIIWW : Vertex, IVertexWWWII
    {
        protected byte _i1;
        public byte I1 => _i1;

        protected byte _i2;
        public byte I2 => _i2;

        protected byte _i3;
        public byte I3 => _i3;

        protected byte _w1;
        public byte W1 => _w1;

        protected byte _w2;
        public byte W2 => _w2;

        public VertexXYZNUVIIIWW(BinaryReader binaryReader) : base(binaryReader)
        {

            _normal = UnpackNormal(binaryReader.ReadInt32());
            _uv0 = binaryReader.ReadVector2();

            _i1 = binaryReader.ReadByte();
            _i2 = binaryReader.ReadByte();
            _i3 = binaryReader.ReadByte();
            _w1 = binaryReader.ReadByte();
            _w2 = binaryReader.ReadByte();
        }
    }
}