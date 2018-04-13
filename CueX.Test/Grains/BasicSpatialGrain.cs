// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.Core;

namespace CueX.Test.Grains
{
    public class BasicSpatialGrainState : SpatialGrainState
    {
    }

    public class BasicSpatialGrain : SpatialGrain<IBasicSpatialGrain, BasicSpatialGrainState>, IBasicSpatialGrain
    {
        public Task<bool> HasParent()
        {
            return Task.FromResult(State.Parent != null);
        }
    }
}