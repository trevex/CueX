// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Geometry
{
    public class Circle : IArea
    {
        
        private Vector3d _origin;
        private readonly double _radius;
        

        private Circle(double radius)
        {
            _radius = radius;
        }

        public static Circle WithRadius(double radius)
        {
            return new Circle(radius);
        }

        public void SetOrigin(Vector3d origin)
        {
            _origin = origin;
        }

        public Vector3d GetOrigin()
        {
            return _origin;
        }

        public double GetHalfBoundingBoxWidth()
        {
            return _radius;
        }

        public bool IsPointInside(Vector3d point)
        {
            return (point - _origin).LengthSq() < (_radius * _radius);
        }
    }
}