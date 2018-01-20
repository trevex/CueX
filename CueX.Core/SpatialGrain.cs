// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.API;
using CueX.MathExt.LinearAlgebra;
using Orleans;

namespace CueX.Core
{
    public abstract class SpatialGrain<TState> : Grain<TState>, ISpatialGrain
        where TState : SpatialGrainState, new()
    {

        public Task<Vector3d> GetPosition()
        {
            return Task.FromResult(State.Position);
        }
    }
}