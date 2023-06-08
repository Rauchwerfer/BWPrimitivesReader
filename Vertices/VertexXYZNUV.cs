using System;
using System.IO;

namespace BWPrimitivesReader.Vertices
{
    [Serializable]
    public class VertexXYZNUV : Vertex
    {
        public VertexXYZNUV(BinaryReader binaryReader) : base(binaryReader)
        {
            _normal = binaryReader.ReadVector3();
            _uv0 = binaryReader.ReadVector2();
        }
    }
}