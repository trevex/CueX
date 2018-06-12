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
        
        public double GetHalfBoundingBoxWidth()
        {
            return _radius;
        }
    }
}