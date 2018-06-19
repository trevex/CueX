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

        public AABB GetBoundingBox()
        {
            return new AABB
            {
                BottomLeft = new Vector3d(_origin.X - _radius, _origin.Y - _radius, 0d),
                TopRight = new Vector3d(_origin.X + _radius, _origin.Y + _radius, 0d)
            };
        }

        public bool IsPointInside(Vector3d point)
        {
            return (point - _origin).LengthSq() < (_radius * _radius);
        }
    }
}