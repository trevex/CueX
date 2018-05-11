// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Geometry
{
    public class Vector3d
    {
        public double[] Data;
        
        public double X
        {
            get => Data[0];
            set => Data[0] = value;
        }
        
        public double Y
        {
            get => Data[1];
            set => Data[1] = value;
        }
        
        public double Z
        {
            get => Data[2];
            set => Data[2] = value;
        }

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

        public override int GetHashCode()
        {
            return (Data != null ? Data.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vector3d other = (Vector3d)obj;
            return Helper.NearlyEqual(Data[0], other.Data[0], double.Epsilon)
                   && Helper.NearlyEqual(Data[1], other.Data[1], double.Epsilon)
                   && Helper.NearlyEqual(Data[2], other.Data[2], double.Epsilon);
        }

        protected bool Equals(Vector3d other)
        {
            return Equals(Data, other.Data);
        }

        public override string ToString()
        {
            return "Vector3d { "+Data[0]+", "+Data[1]+", "+Data[2]+" }";
        }
    }
}