// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;

namespace CueX.Core
{
    public abstract class SpatialPubSubBuilder
    {
        
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

        public abstract Task<ISpatialPubSub> Build(IClusterClient client);
    }
}