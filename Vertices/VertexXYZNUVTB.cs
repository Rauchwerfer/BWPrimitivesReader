using System;
using System.IO;
using System.Numerics;

namespace BWPrimitivesReader.Vertices
{
    [Serializable]
    public class VertexXYZNUVTB : Vertex, IVertexTB
    {
        protected Vector4 _tangent;
        /// <summary>
        /// Propapbly must be unpacked, but behaves weird and this because is unused
        /// </summary>
        public Vector4 Tangent => _tangent;

        protected Vector4 _bitangent;
        /// <summary>
        /// Propapbly must be unpacked, but behaves weird and this because is unused
        /// </summary>
        public Vector4 Bitangent => _bitangent;

        public VertexXYZNUVTB(BinaryReader binaryReader) : base(binaryReader)
        {
            _normal = UnpackNormal(binaryReader.ReadInt32());
            _uv0 = binaryReader.ReadVector2();

            Vector3 tangentVec3 = UnpackNormal(binaryReader.ReadInt32());
            Vector3 bitangentVec3 = UnpackNormal(binaryReader.ReadInt32());

            _tangent = new Vector4(tangentVec3, 1f);
            _bitangent = new Vector4(bitangentVec3, 1f);
        }
    }
}