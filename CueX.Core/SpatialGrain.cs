// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.Numerics;
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
    public abstract class SpatialGrain<TGrainInterface, TState> : Grain<TState>, ISpatialGrain
        where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain
    {
        public async Task SetPosition(Vector3d newPosition)
        {
            State.Position = newPosition;
            await WriteStateAsync();
        }

        public Task<Vector3d> GetPosition()
        {
            return Task.FromResult(State.Position);
        }

        public async Task SetParent<T>(T parent) where T : IPartitionGrain
        {
            State.Parent = parent;
            await WriteStateAsync();
        }

        public Task<bool> RemoveSelfFromParent()
        {
            return State.Parent.Remove(this.AsReference<TGrainInterface>());
        }
    }
}