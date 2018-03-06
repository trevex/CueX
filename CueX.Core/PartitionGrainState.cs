// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using CueX.API;
using CueX.Numerics.Projection;

namespace CueX.Core
{
    public abstract class PartitionGrainState
    {
        public List<ISpatialGrain> Children = new List<ISpatialGrain>();
    }
}