// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.Core;
using CueX.Numerics;

namespace SimpleExample.Grains
{
    public class SimpleGrainState : SpatialGrainState
    {
    }

    public class SimpleGrain : SpatialGrain<SimpleGrainState>, ISimpleGrain
    {
        public async Task SetPosition(Vector3d newPosition)
        {
            State.Position = newPosition;
            await WriteStateAsync();
        }
    }
}