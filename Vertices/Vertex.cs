using System;
using System.IO;
using System.Numerics;

namespace BWPrimitivesReader.Vertices
{
    [Serializable]
    public abstract class Vertex
    {
        protected Vector3 _position = Vector3.Zero;
        protected Vector2 _uv0 = Vector2.Zero;
        protected Vector3 _normal = Vector3.Zero;

        public Vector3 Position => _position;
        public Vector2 UV0 => _uv0;
        public Vector2 UV0FlippedVertically => new Vector2(_uv0.X, -_uv0.Y);
        public Vector3 Normal => _normal;

        public Vertex(Vector3 position, Vector2 uv0, Vector3 normal)
        {
            _position = position;
            _uv0 = uv0;
            _normal = normal;
        }

        public Vertex(Vector3 position, Vector2 uv0, int normal)
        {
            _position = position;
            _uv0 = uv0;
            _normal = UnpackNormal(normal);
        }

        /// <summary>
        /// Base constructor of vertex, initialize only position of the vertex since all vertices have 
        /// their own type of normal (Packed in Int32 or 3 Single values)
        /// </summary>
        /// <param name="binaryReader"></param>
        public Vertex(BinaryReader binaryReader)
        {
            _position = binaryReader.ReadVector3();
        }

        /// <summary>
        /// Unpack normal Vector3 from Int32
        /// </summary>
        /// <param name="packed"></param>
        /// <returns></returns>
        protected static Vector3 UnpackNormal(int packed)
        {
            int pkz = (packed >> 22) & 0x3FF;
            int pky = (packed >> 11) & 0x7FF;
            int pkx = packed & 0x7FF;
            float x, y, z;

            if (pkx > 0x3ff)
                x = -(float)((pkx & 0x3ff ^ 0x3ff) + 1) / 0x3ff;
            else
                x = (float)pkx / 0x3ff;

            if (pky > 0x3ff)
                y = -(float)((pky & 0x3ff ^ 0x3ff) + 1) / 0x3ff;
            else
                y = (float)pky / 0x3ff;

            if (pkz > 0x1ff)
                z = -(float)((pkz & 0x1ff ^ 0x1ff) + 1) / 0x1ff;
            else
                z = (float)pkz / 0x1ff;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// [unused] Unpack normal Vector3 from Int32
        /// </summary>
        /// <param name="packed"></param>
        /// <returns></returns>
        protected static Vector3 UnpackNormalTag3(int packed)
        {
            int pkz = (packed >> 16 & 0xFF ^ 0xFF);
            int pky = (packed >> 8 & 0xFF ^ 0xFF);
            int pkx = (packed & 0xFF ^ 0xFF);
            float x, y, z;

            if (pkx > 0x7f)
                x = -(float)(pkx & 0x7f) / 0x7f;
            else
                x = (float)(pkx ^ 0x7f) / 0x7f;

            if (pky > 0x7f)
                y = -(float)(pky & 0x7f) / 0x7f;
            else
                y = (float)(pky ^ 0x7f) / 0x7f;

            if (pkz > 0x7f)
                z = -(float)(pkz & 0x7f) / 0x7f;
            else
                z = (float)(pkz ^ 0x7f) / 0x7f;

            return new Vector3(x, y, z);
        }
    }
}