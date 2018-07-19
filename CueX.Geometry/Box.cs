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

        public AABB GetBoundingBox(Vector3d origin)
        {
            var hw = _width * 0.5d;
            var hh = _height * 0.5d;
            return new AABB
            {
                BottomLeft = new Vector3d(origin.X - hw, origin.Y - hh, 0d),
                TopRight = new Vector3d(origin.X + hw, origin.Y + hh, 0d)
            };
        }

        public bool IsPointInside(Vector3d origin, Vector3d point)
        {
            var os = point - origin;
            var hw = _width * 0.5d;
            var hh = _height * 0.5d;
            return !(os.X > hw || os.X < -hw || os.Y > hh || os.Y < -hh);
        }
    }
}