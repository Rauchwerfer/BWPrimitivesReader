using System;
using System.Collections.Generic;
using System.IO;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// The section with index data contains a small header and the raw index data, followed by the primitive
    /// groups.The primitive groups define the batches with different materials in the triangle list (these are the
    /// same primitive groups referenced from the .visual file).
    /// </summary>
    [Serializable]
    public class IndexDataSection : BinSection
    {
        public int NumberOfIndices { get; private set; } = 0;
        public BWPrimitiveGroupData[] PrimitiveData { get; private set; } = Array.Empty<BWPrimitiveGroupData>();
        public int[] Indices { get; private set; } = Array.Empty<int>();

        public IndexDataSection(BinaryReader binaryReader)
        {
            ReadFormat(binaryReader);

            switch (Format)
            {
                case "list":
                    ReadIndexData(binaryReader, 2);
                    break;
                case "list32":
                    ReadIndexData(binaryReader, 4);
                    break;
            }
        }

        private void ReadIndexData(BinaryReader binaryReader, int uintLen)
        {
            List<int> indices = new List<int>();
            NumberOfIndices = binaryReader.ReadInt32();

            int number_of_primitive_groups = binaryReader.ReadInt32();

            byte[] raw_index_data = binaryReader.ReadBytes(NumberOfIndices * uintLen);


            PrimitiveData = new BWPrimitiveGroupData[number_of_primitive_groups];

            for (int i = 0; i < number_of_primitive_groups; i++)
            {
                int start_idx = binaryReader.ReadInt32();
                int num_of_primtvs = binaryReader.ReadInt32();
                int start_vrtx = binaryReader.ReadInt32();
                int num_of_vrtcs = binaryReader.ReadInt32();

                PrimitiveData[i] = new BWPrimitiveGroupData(start_idx, num_of_primtvs, start_vrtx, num_of_vrtcs);
            }

            using (MemoryStream ms = new MemoryStream(raw_index_data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    if (uintLen == 2)
                    {
                        for (int i = 0; i < (raw_index_data.Length / uintLen); i++)
                        {

                            int v1 = br.ReadUInt16();

                            indices.Add(v1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < (raw_index_data.Length / uintLen); i++)
                        {

                            int v1 = br.ReadInt32();

                            indices.Add(v1);
                        }
                    }
                }
            }

            Indices = indices.ToArray();
        }

        [Serializable]
        public class BWPrimitiveGroupData
        {
            public int StartIndex { get; }
            public int NumberOfPrimitives { get; }
            public int StartVertex { get; }
            public int NumberOfVertices { get; }

            public BWPrimitiveGroupData(int startIdx, int numOfPrimtvs, int startVrtx, int numOfVrtcs)
            {
                StartIndex = startIdx;             // First index used by this group of triangles.
                NumberOfPrimitives = numOfPrimtvs;   // Number of triangles rendered in this group
                StartVertex = startVrtx;           // First vertex used by the triangles in this group.
                NumberOfVertices = numOfVrtcs;       // Number of vertices used by the triangles in this group
            }

            public override string ToString()
            {
                return $"StartIndex {StartIndex}; NumberOfPrimitives = {NumberOfPrimitives}; StartVertex = {StartVertex};  NumberOfVertices = {NumberOfVertices};";
            }
        }
    }
}