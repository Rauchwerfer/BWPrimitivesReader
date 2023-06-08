using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// [unused] The section with Binary Space Partitioning data.
    /// </summary>
    [Serializable]
    public class BSP2DataSection : BinSection
    {
        public List<(Vector3, Vector3, Vector3)> Triangles { get; } = new List<(Vector3, Vector3, Vector3)>();

        public BSP2DataSection(BinaryReader binaryReader)
        {
            // <header>
            int magic = binaryReader.ReadInt32();
            uint num_triangles = binaryReader.ReadUInt32();
            uint max_triangles = binaryReader.ReadUInt32();
            uint num_nodes = binaryReader.ReadUInt32();

            //num_triangles = num_triangles > max_triangles ? max_triangles : num_triangles;

            //<triangle>*

            for (int i = 0; i < num_triangles; i++)
            {
                float x = binaryReader.ReadSingle();
                float y = binaryReader.ReadSingle();
                float z = binaryReader.ReadSingle();

                Vector3 vertexPos1 = new Vector3(x, y, z);

                x = binaryReader.ReadSingle();
                y = binaryReader.ReadSingle();
                z = binaryReader.ReadSingle();

                Vector3 vertexPos2 = new Vector3(x, y, z);

                x = binaryReader.ReadSingle();
                y = binaryReader.ReadSingle();
                z = binaryReader.ReadSingle();

                Vector3 vertexPos3 = new Vector3(x, y, z);

                Triangles.Add((vertexPos1, vertexPos2, vertexPos3));
            }

            ////<node>*
            //for (int i = 0; i < num_nodes; i++)
            //{
            //    // <node_flags> = <reserved> <is_partitioned> <has_front> <has_back>
            //    // <reserved>           5-bit reserved number 10100.
            //    // <has_back>           1-bit flag indicating if node has a back child.
            //    // <has_front>          1-bit flag indicating if node has a front child.
            //    // <is_partitioned>     1-bit flag indicating if all triangles lie on the node's plane
            //    byte node_flags = binaryReader.ReadByte();

            //    // <plane_eq>
            //    float x = binaryReader.ReadSingle();
            //    float y = binaryReader.ReadSingle();
            //    float z = binaryReader.ReadSingle();
            //    float d = binaryReader.ReadSingle();

            //    // <num_indices>
            //    ushort num_indices = binaryReader.ReadUInt16();

            //    ushort[] triangle_indices = new ushort[num_indices];
            //    // <triangle_index>*
            //    for (int j = 0; j < num_indices; j++)
            //    {
            //        triangle_indices[j] = binaryReader.ReadUInt16();
            //    }
            //}

            //// <user_data> 
            //uint user_data_key = binaryReader.ReadUInt32();
            //uint blob_size = binaryReader.ReadUInt32();

            //byte[] blob = new byte[blob_size];

            //for (int j = 0; j < blob_size; j++)
            //{
            //    blob[j] = binaryReader.ReadByte();
            //}
        }
    }
}