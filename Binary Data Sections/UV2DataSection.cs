using System;
using System.IO;
using System.Numerics;

namespace BWPrimitivesReader.BinaryDataSections
{
    /// <summary>
    /// The section with second UV data. 
    /// </summary>
    [Serializable]
    public class UV2DataSection : BinSection
    {
        public Vector2[] SecondUV { get; }

        public UV2DataSection(BinaryReader binaryReader, int numberOfVertices)
        {
            long initialPosition = binaryReader.BaseStream.Position;

            //// Subname
            //byte[] uv2_subname_bytes = reader.ReadBytes(64);

            //int uv2_subname_stringEndPos = 0;

            //for (int i = 0; i < uv2_subname_bytes.Length; i++)
            //{
            //    if (uv2_subname_bytes[i] == 0x00) { uv2_subname_stringEndPos = i; break; }
            //}

            //try
            //{
            //    var result = new UTF8Encoding(false, true).GetString(uv2_subname_bytes, 0, uv2_subname_stringEndPos);
            //    uv2ds.uv2_subname = result;
            //}
            //catch
            //{
            //    reader.BaseStream.Seek(initialPosition, SeekOrigin.Begin);
            //    uv2ds.uv2_subname = "uv2_None";
            //}




            string uv2_format = string.Empty;

            //if (uv2ds.uv2_subname.Contains("BPVS"))
            //{
            //    reader.ReadBytes(4);

            //    byte[] uv2_format_bytes = reader.ReadBytes(64);

            //    int uv2_format_stringEndPos = 0;

            //    for (int i = 0; i < uv2_format_bytes.Length; i++)
            //    {
            //        if (uv2_format_bytes[i] == 0x00) { uv2_format_stringEndPos = i; break; }
            //    }

            //    uv2_format = Encoding.UTF8.GetString(uv2_format_bytes, 0, uv2_format_stringEndPos);
            //}

            if (uv2_format == "set3/uv2pc")
            {
                int uv2Length = binaryReader.ReadInt32();

                Vector2[] uv2list = new Vector2[uv2Length];

                for (int j = 0; j < uv2Length; j++)
                {
                    float u = binaryReader.ReadSingle();
                    float v = binaryReader.ReadSingle();

                    uv2list[j] = new Vector2(u, 1 - v);
                }

                SecondUV = uv2list;
            }
            else /*if (uv2ds.uv2_subname == "uv2_None")*/
            {
                SecondUV = new Vector2[numberOfVertices];

                for (int j = 0; j < numberOfVertices; j++)
                {
                    float u = binaryReader.ReadSingle();
                    float v = binaryReader.ReadSingle();

                    SecondUV[j] = new Vector2(u, 1 - v);
                }
            }
        }
    }
}