// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;

namespace CueX.Numerics.Projection
{
    public class WebMercator : ICoordinateProjection
    {
        private const int EarthRadius = 6378137;
        private const double OriginShift = 2 * Math.PI * EarthRadius / 2;

        /// <summary>
        /// Converts a LatLng instance to a Vector where XY correstpond to Spherical Mercator
        /// </summary>
        /// <param name="latLng"></param>
        /// <returns></returns>
        public Vector3d FromLatLng(LatLng latLng)
        {
            var p = new Vector3d
            {
                X = latLng.Lng * OriginShift / 180,
                Y = Math.Log(Math.Tan((90 + latLng.Lat) * Math.PI / 360)) / (Math.PI / 180)
            };
            p.Y = p.Y * OriginShift / 180;
            return p;
        }
    }
}