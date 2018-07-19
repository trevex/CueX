// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Geometry
{
    public class Circle : IArea
    {
        private readonly double _radius;

        private Circle(double radius)
        {
            _radius = radius;
        }

        public static Circle WithRadius(double radius)
        {
            return new Circle(radius);
        }

        public AABB GetBoundingBox(Vector3d origin)
        {
            return new AABB
            {
                BottomLeft = new Vector3d(origin.X - _radius, origin.Y - _radius, 0d),
                TopRight = new Vector3d(origin.X + _radius, origin.Y + _radius, 0d)
            };
        }

        public bool IsPointInside(Vector3d origin, Vector3d point)
        {
            return (point - origin).LengthSq() < (_radius * _radius);
        }
    }
}