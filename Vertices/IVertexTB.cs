using System.Numerics;

namespace BWPrimitivesReader.Vertices
{
    public interface IVertexTB
    {
        public Vector4 Tangent { get; }
        public Vector4 Bitangent { get; }
    }
}