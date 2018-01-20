// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Numerics;
using CueX.API;
using Orleans;

namespace CueX.Core
{
    public class SpatialGrain<TState, TPrecision> : Grain<TState>, ISpatialGrain<TPrecision>
        where TPrecision : struct
        where TState : SpatialGrainState<TPrecision>, new()
    {

        public Vector<TPrecision> GetPosition()
        {
            return State.Position;
        }
    }
}