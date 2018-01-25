// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Numerics;

namespace CueX.Core
{
    public abstract class SpatialPubSubClient
    {
        public void InitializeCommon()
        {
            if (!Vector.IsHardwareAccelerated)
            {
                // TODO: throw warning
            }
        }
    }
}