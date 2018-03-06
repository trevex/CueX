// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.API;
using CueX.Numerics;
using CueX.Numerics.Projection;
using Orleans;

// NOTE: SpatialGrain's position is in the Format that the ICoordinateProjection specifies,
//       therefore all helper, e.g. metric tooling, need to use the conversion functions provided by the projection

namespace CueX.Core
{
    /// <summary>
    /// Abstract class providing the basic functionality necessary for sub-classes
    /// to be insertable into the PubSub-System.
    /// </summary>
    /// <typeparam name="TState">Application-specific state data type, that also holds <see cref="SpatialGrainState"/>.</typeparam>
    public abstract class SpatialGrain<TState> : Grain<TState>, ISpatialGrain
        where TState : SpatialGrainState, new()
    {

        public Task SetCoordinateProjection(ICoordinateProjection coordinateProjection)
        {
            State.CoordinateProjection = coordinateProjection;
            return WriteStateAsync();
        }
        
        public Task<Vector3d> GetPosition()
        {
            return Task.FromResult(State.Position);
        }

        public Task SetParent<T>(T parent) where T : IPartitionGrain
        {
            return Task.CompletedTask;
        }
    }
}