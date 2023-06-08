using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BWPrimitivesReader.BinaryDataSections;

namespace BWPrimitivesReader
{
    /// <summary>
    /// The primitive file can contain vertex data, index data, uv2 data and BSP data.
    /// </summary>
    [Serializable]
    public class BWPrimitives
    {
        public List<DataSectionEntry> PackedSectionEntries { get; } = new List<DataSectionEntry>();

        private int _numberOfVertices = 0;

        private string[] _vertexSectionsNames;
        private string[] _indexSectionsNames;
        private string[] _uvSectionsNames;

        public BWPrimitives(string filename, List<string> vertexSections, List<string> indexSections, List<string> uvSections)
            : this(File.ReadAllBytes(filename), vertexSections, indexSections, uvSections)
        {

        }

        public BWPrimitives(byte[] primitivesBytes, List<string> vertexSections, List<string> indexSections, List<string> uvSections)
        {
            _vertexSectionsNames = vertexSections.ToArray();
            _indexSectionsNames = indexSections.ToArray();
            _uvSectionsNames = uvSections.ToArray();

            using MemoryStream ms = new MemoryStream(primitivesBytes);
            using (BinaryReader binaryReader = new BinaryReader(ms))
            {
                int magic = binaryReader.ReadInt32();

                if (magic != 0x42a14e65)
                    throw new FileLoadException("Bytes do not contain .primitives file!");

                ReadIndexTable(binaryReader);

                foreach (var packedSectionEntry in PackedSectionEntries)
                {
                    ReadPackedSectionEntry(binaryReader, packedSectionEntry);
                }

            }
        }

        private void ReadPackedSectionEntry(BinaryReader reader, DataSectionEntry dataSectionEntry)
        {
            reader.BaseStream.Position = dataSectionEntry.Position;

            if (_vertexSectionsNames.Contains(dataSectionEntry.Name))
            {
                dataSectionEntry.SetBinarySection(new VertexDataSection(reader));

                _numberOfVertices = ((VertexDataSection)dataSectionEntry.BinarySection).NumberOfVertices;

                return;
            }

            if (_uvSectionsNames.Contains(dataSectionEntry.Name))
            {
                dataSectionEntry.SetBinarySection(new UV2DataSection(reader, _numberOfVertices));

                return;
            }

            if (_indexSectionsNames.Contains(dataSectionEntry.Name))
            {
                dataSectionEntry.SetBinarySection(new IndexDataSection(reader));

                return;
            }

            if (dataSectionEntry.Name == "bsp2")
            {
                dataSectionEntry.SetBinarySection(new BSP2DataSection(reader));

                return;
            }

            if (dataSectionEntry.Name == "bsp2_materials")
            {
                dataSectionEntry.SetBinarySection(new BSP2MaterialsSection(reader, dataSectionEntry.BlobLength));

                return;
            }
        }

        /// <summary>
        /// Read Index Table in the end of the primitives file. 
        /// </summary>
        /// <param name="binaryReader"></param>
        private void ReadIndexTable(BinaryReader binaryReader)
        {
            PackedSectionEntries.Clear();

            long len = binaryReader.BaseStream.Length;


            binaryReader.BaseStream.Position = len - 4;

            int index_table_length = binaryReader.ReadInt32();

            int offset = (int)len - (index_table_length + 4); // length of section <index_table>

            binaryReader.BaseStream.Position = offset;

            int dataLengthCounter = 4;

            while (binaryReader.BaseStream.Position < len - 4)
            {
                ReadDataSectionEntry(binaryReader, ref dataLengthCounter);
            }
        }

        private void ReadDataSectionEntry(BinaryReader binaryReader, ref int dataLengthCounter)
        {
            //  <blob_length>
            int blob_length = binaryReader.ReadInt32();     // 4-byte little endian integer, containing the length of respective <binary_blob>

            // <reserved_data>
            uint preloadLen = binaryReader.ReadUInt32();    // uint32 containing length of data when streamed in
            uint version = binaryReader.ReadUInt32();       // uint32 containing a version number
            ulong modified = binaryReader.ReadUInt64();    //  uint64 containing timestamp of last modification

            //<data_section_tag>
            int tag_length = binaryReader.ReadInt32();    // 4-byte little endian integer, containing length of the section's tag.

            int tag_length_without_nulls = tag_length;

            tag_length = PadIntTo4ByteBoundary(tag_length);

            //int tag_value = binaryReader.ReadInt32();     // Section's tag, padded to 4-byte boundary
            byte[] tag_value = binaryReader.ReadBytes(tag_length);

            string name = Encoding.UTF8.GetString(tag_value, 0, tag_length_without_nulls);

            PackedSectionEntries.Add(new DataSectionEntry(name, blob_length, dataLengthCounter));

            dataLengthCounter += PadIntTo4ByteBoundary(blob_length);
        }

        private int PadIntTo4ByteBoundary(int value)
        {
            int remainder = value % 4;

            if (remainder > 0)
            {
                value = value - remainder + 4;
            }

            return value;
        }

        /// <summary>
        /// *.primitives file section information (name, length and position), 
        /// and it's actual deseralized binary section object.
        /// </summary>
        [Serializable]
        public class DataSectionEntry
        {
            public string Name { get; }
            public int BlobLength { get; }
            public int Position { get; }

            public BinSection? BinarySection { get; private set; }

            public DataSectionEntry(string name, int blobLength, int position)
            {
                Name = name;
                BlobLength = blobLength;
                Position = position;
            }

            public void SetBinarySection(BinSection binarySection)
            {
                BinarySection = binarySection;
            }

            public override string ToString()
            {

                return $"Section \"{Name}\": {Environment.NewLine}" +
                    $"\tblob length: {BlobLength}{Environment.NewLine}" +
                    $"\tposition: {Position}{Environment.NewLine}" +
                    $"\ttype of section: {BinarySection?.GetType()}{Environment.NewLine}";
            }
        }
    }
}