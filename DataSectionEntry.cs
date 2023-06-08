using System;
using BWPrimitivesReader.BinaryDataSections;

namespace BWPrimitivesReader
{    
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

        public BinSection BinarySection { get; private set; }

        public DataSectionEntry(string name, int blobLength, int position)
        {
            this.Name = name;
            this.BlobLength = blobLength;
            this.Position = position;
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