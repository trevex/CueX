using System;
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