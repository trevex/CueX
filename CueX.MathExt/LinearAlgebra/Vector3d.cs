using System;

namespace CueX.MathExt.LinearAlgebra
{
    public class Vector3d
    {
        public double[] Data;

        public Vector3d(double x, double y, double z)
        {
            Data = new double[]{ x, y, z };
        }

        public Vector3d() : this(0.0d, 0.0d, 0.0d)
        {
        }

        public static Vector3d One()
        {
            return new Vector3d(1.0d, 1.0d, 1.0d);
        }

        public static Vector3d Zero()
        {
            return new Vector3d();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vector3d other = (Vector3d)obj;
            return Helper.NearlyEqual(Data[0], other.Data[0], Double.Epsilon)
                   && Helper.NearlyEqual(Data[1], other.Data[1], Double.Epsilon)
                   && Helper.NearlyEqual(Data[2], other.Data[2], Double.Epsilon);
        }
    }
}