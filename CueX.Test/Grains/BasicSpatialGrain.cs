// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.Core;
using CueX.MathExt;
using CueX.MathExt.LinearAlgebra;

namespace CueX.Test.Grains
{
    public class BasicSpatialGrainState : SpatialGrainState
    {
    }

    public class BasicSpatialGrain : SpatialGrain<BasicSpatialGrainState>, IBasicSpatialGrain
    {
        public async Task SetPosition(Vector3d newPosition)
        {
            State.Position = newPosition;
            await WriteStateAsync();
        }
    }
}