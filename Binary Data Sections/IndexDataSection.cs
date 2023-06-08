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
        private int _numberOfIndices = 0;
        public int NumberOfIndices => _numberOfIndices;

        private BWPrimitiveGroupData[] _rawPrimitiveData;

        public BWPrimitiveGroupData[] PrimitiveData => _rawPrimitiveData;

        public readonly List<int> indices = new List<int>();

        public IndexDataSection(BinaryReader binaryReader)
        {
            ReadFormat(binaryReader);

            switch (_format)
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
            _numberOfIndices = binaryReader.ReadInt32();

            int number_of_primitive_groups = binaryReader.ReadInt32();

            byte[] raw_index_data = binaryReader.ReadBytes(_numberOfIndices * uintLen);


            _rawPrimitiveData = new BWPrimitiveGroupData[number_of_primitive_groups];

            for (int i = 0; i < number_of_primitive_groups; i++)
            {
                int start_idx = binaryReader.ReadInt32();
                int num_of_primtvs = binaryReader.ReadInt32();
                int start_vrtx = binaryReader.ReadInt32();
                int num_of_vrtcs = binaryReader.ReadInt32();

                _rawPrimitiveData[i] = new BWPrimitiveGroupData(start_idx, num_of_primtvs, start_vrtx, num_of_vrtcs);
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
        }

        [Serializable]
        public class BWPrimitiveGroupData
        {
            private readonly int _start_idx;
            private readonly int _num_of_primtvs;
            private readonly int _start_vrtx;
            private readonly int _num_of_vrtcs;

            public int StartIndex => _start_idx;
            public int NumberOfPrimitives => _num_of_primtvs;
            public int StartVertex => _start_vrtx;
            public int NumberOfVertices => _num_of_vrtcs;

            public BWPrimitiveGroupData(int start_idx, int num_of_primtvs, int start_vrtx, int num_of_vrtcs)
            {
                _start_idx = start_idx;             // First index used by this group of triangles.
                _num_of_primtvs = num_of_primtvs;   // Number of triangles rendered in this group
                _start_vrtx = start_vrtx;           // First vertex used by the triangles in this group.
                _num_of_vrtcs = num_of_vrtcs;       // Number of vertices used by the triangles in this group
            }

            public override string ToString()
            {
                return $"start_idx {_start_idx}; num_of_primtvs = {_num_of_primtvs}; start_vrtx = {_start_vrtx};  num_of_vrtcs = {_num_of_vrtcs}; {Environment.NewLine}";
            }
        }
    }
}