// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using System.Numerics;
using CueX.API;
using CueX.Numerics.Projection;

namespace CueX.Core
{
    public abstract class SpatialPubSubBuilder
    {
        private ICoordinateProjection _coordinateProjection;
        
        protected bool CheckHardwareSupport()
        {
            if (!Vector.IsHardwareAccelerated)
            {
                // TODO: throw warning
                Console.WriteLine("Hardware acceleration for Vectors is unavailable!");
                return false;
            }

            return true;
        }

        public SpatialPubSubBuilder UseCoordinateProjection(ICoordinateProjection coordinateProjection)
        {
            _coordinateProjection = coordinateProjection;
            return this;
        }

        public abstract ISpatialPubSub Build();
    }
}