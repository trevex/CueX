// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;

namespace CueX.Geometry
{
    public class Box : IArea
    {
        private readonly double _width;
        private readonly double _height;

        private Box(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public static Box WithSize(double size)
        {
            return new Box(size, size);
        }

        public static Box WithDimensions(double width, double height)
        {
            return new Box(width, height);
        }
        
        public double GetHalfBoundingBoxWidth()
        {
            var size = Math.Max(_width, _height);
            return size * 0.5d;
        }
    }
}