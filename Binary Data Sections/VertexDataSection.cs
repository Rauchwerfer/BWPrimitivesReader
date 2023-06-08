using System;
using System.Collections.Generic;
using System.IO;
using BWPrimitivesReader.Vertices;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// The section with vertex data contains a small header, followed by the raw vertex data.
    /// </summary>
    [Serializable]
    public class VertexDataSection : BinSection
    {
        public Vertex[] Vertices { get; } = Array.Empty<Vertex>();
        public bool IsSkinned { get; } = false;
        public bool HasTangents { get; } = false;
        public int NumberOfVertices => Vertices.Length;

        private Dictionary<string, Type> _vertexTypes = new Dictionary<string, Type>()
        {
            { "xyznuvtb", typeof(VertexXYZNUVTB) },
            { "xyznuv", typeof(VertexXYZNUV) },
            { "xyznuviiiwwtb", typeof(VertexXYZNUVIIIWWTB) },
            { "xyznuviiiww", typeof(VertexXYZNUVIIIWW) },
            { "xyznuvi", typeof(VertexXYZNUVI) }
        };

        public VertexDataSection(BinaryReader binaryReader)
        {
            ReadFormat(binaryReader);

            int numberOfVertices = binaryReader.ReadInt32();

            try
            {
                var vertexType = _vertexTypes[Format];

                HasTangents = typeof(IVertexTB).IsAssignableFrom(vertexType);
                IsSkinned = typeof(IVertexWWWII).IsAssignableFrom(vertexType);

                Vertices = (Vertex[])Array.CreateInstance(vertexType, numberOfVertices);

                for (int i = 0; i < numberOfVertices; i++)
                {
                    Vertices[i] = (Vertex)Activator.CreateInstance(vertexType, args: binaryReader);
                }
            }
            catch(KeyNotFoundException)
            {
                throw new Exception($"Unknown vertex type \"{Format}!\"");
            }
            

            //switch (Format)
            //{
            //    case "xyznuvtb":
            //        HasTangents = true;
            //        IsSkinned = false;
            //        Vertices = new VertexXYZNUVTB[numberOfVertices];

            //        for (int i = 0; i < numberOfVertices; i++)
            //        {
            //            Vertices[i] = new VertexXYZNUVTB(binaryReader);
            //        }

            //        break;
            //    case "xyznuv":
            //        HasTangents = false;
            //        IsSkinned = false;

            //        Vertices = new VertexXYZNUV[numberOfVertices];

            //        for (int i = 0; i < numberOfVertices; i++)
            //        {
            //            Vertices[i] = new VertexXYZNUV(binaryReader);
            //        }

            //        break;
            //    case "xyznuviiiwwtb":
            //        HasTangents = true;
            //        IsSkinned = true;

            //        Vertices = new VertexXYZNUVIIIWWTB[numberOfVertices];

            //        for (int i = 0; i < numberOfVertices; i++)
            //        {
            //            Vertices[i] = new VertexXYZNUVIIIWWTB(binaryReader);
            //        }

            //        break;
            //    case "xyznuviiiww":
            //        HasTangents = false;
            //        IsSkinned = true;

            //        Vertices = new VertexXYZNUVIIIWW[numberOfVertices];

            //        for (int i = 0; i < numberOfVertices; i++)
            //        {
            //            Vertices[i] = new VertexXYZNUVIIIWW(binaryReader);
            //        }

            //        break;
            //    case "xyznuvi":
            //        HasTangents = false;
            //        IsSkinned = false;

            //        Vertices = new VertexXYZNUVI[numberOfVertices];

            //        for (int i = 0; i < numberOfVertices; i++)
            //        {
            //            Vertices[i] = new VertexXYZNUVI(binaryReader);
            //        }
            //        break;
            //}
        }
    }
}

