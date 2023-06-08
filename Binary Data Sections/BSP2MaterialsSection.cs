using System;
using System.IO;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// [unused] BSP2 Materials
    /// </summary>
    [Serializable]
    public class BSP2MaterialsSection : BinSection
    {
        public string BSP2Materials { get; }

        public BSP2MaterialsSection(BinaryReader binaryReader, int blobLength)
        {
            char[] chars = binaryReader.ReadChars(blobLength);

            BSP2Materials = new string(chars);
        }
    }
}