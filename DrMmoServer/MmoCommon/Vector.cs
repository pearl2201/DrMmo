using DarkRift;
using System;
using System.Collections.Generic;
using System.Text;

namespace MmoCommon
{
    public struct Vector : IDarkRiftSerializable
    {
        public const float TOLERANCE = 0.000001f;

        public static Vector Zero;

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vector(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(float x, float y) : this()
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public Vector(Vector v) : this()
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        public static Vector operator /(Vector a, int b)
        {
            return new Vector { X = a.X / b, Y = a.Y / b, Z = a.Z / b };
        }

        public static Vector operator *(Vector a, float b)
        {
            return new Vector { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector operator *(Vector a, int b)
        {
            return new Vector { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        public static Vector operator -(Vector a)
        {
            return new Vector { X = -a.X, Y = -a.Y, Z = -a.Z };
        }

        public static Vector Max(Vector value1, Vector value2)
        {
            return new Vector { X = Math.Max(value1.X, value2.X), Y = Math.Max(value1.Y, value2.Y), Z = Math.Max(value1.Z, value2.Z) };
        }

        public static Vector Min(Vector value1, Vector value2)
        {
            return new Vector { X = Math.Min(value1.X, value2.X), Y = Math.Min(value1.Y, value2.Y), Z = Math.Min(value1.Z, value2.Z) };
        }

        public override string ToString()
        {
            return string.Format("{0}({1:0.00}, {2:0.00}, {3:0.00})", "V", X, Y, Z);
        }

        public bool IsZero
        {
            get { return Math.Abs(this.X) < TOLERANCE && Math.Abs(this.Y) < TOLERANCE && Math.Abs(this.Z) < TOLERANCE; }
        }

        public float Len2
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public void Deserialize(DeserializeEvent e)
        {
            X = e.Reader.ReadSingle();
            Y = e.Reader.ReadSingle();
            Z = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(X);
            e.Writer.Write(Y);
            e.Writer.Write(Z);
        }
    }
}
