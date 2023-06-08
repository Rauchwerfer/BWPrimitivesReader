using BWPrimitivesReader.Vertices;
using System;
using System.IO;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// The section with vertex data contains a small header, followed by the raw vertex data.
    /// </summary>
    [Serializable]
    public class VertexDataSection : BinSection
    {
        public readonly Vertex[] vertices;
        public bool IsSkinned { get; } = false;
        public bool HasTangents { get; } = false;
        public int NumberOfVertices => vertices.Length;

        public VertexDataSection(BinaryReader binaryReader)
        {
            ReadFormat(binaryReader);

            int numberOfVertices = binaryReader.ReadInt32();

            switch (_format)
            {
                case "xyznuvtb":
                    HasTangents = true;
                    IsSkinned = false;
                    vertices = new VertexXYZNUVTB[numberOfVertices];

                    for (int i = 0; i < numberOfVertices; i++)
                    {
                        vertices[i] = new VertexXYZNUVTB(binaryReader);
                    }

                    break;
                case "xyznuv":
                    HasTangents = false;
                    IsSkinned = false;

                    vertices = new VertexXYZNUV[numberOfVertices];

                    for (int i = 0; i < numberOfVertices; i++)
                    {
                        vertices[i] = new VertexXYZNUV(binaryReader);
                    }

                    break;
                case "xyznuviiiwwtb":
                    HasTangents = true;
                    IsSkinned = true;

                    vertices = new VertexXYZNUVIIIWWTB[numberOfVertices];

                    for (int i = 0; i < numberOfVertices; i++)
                    {
                        vertices[i] = new VertexXYZNUVIIIWWTB(binaryReader);
                    }

                    break;
                case "xyznuviiiww":
                    HasTangents = false;
                    IsSkinned = true;

                    vertices = new VertexXYZNUVIIIWW[numberOfVertices];

                    for (int i = 0; i < numberOfVertices; i++)
                    {
                        vertices[i] = new VertexXYZNUVIIIWW(binaryReader);
                    }

                    break;
                case "xyznuvi":
                    HasTangents = false;
                    IsSkinned = false;

                    vertices = new VertexXYZNUVI[numberOfVertices];

                    for (int i = 0; i < numberOfVertices; i++)
                    {
                        vertices[i] = new VertexXYZNUVI(binaryReader);
                    }
                    break;
            }
        }
    }
}

