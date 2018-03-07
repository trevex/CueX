// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.API;
using Orleans;

namespace CueX.Core
{
    /// <summary>
    /// Abstract class providing the basic functionality necessary for sub-classes
    /// to manage a partition of the SPS area.
    /// </summary>
    /// <typeparam name="TState">Application-specific state data type, that also holds <see cref="PartitionGrainState"/>.</typeparam>
    public abstract class PartitionGrain<TState> : Grain<TState>, IPartitionGrain
        where TState : PartitionGrainState, new()
    {
        
        public abstract Task Add<T>(T spatialGrain) where T : ISpatialGrain;

        public abstract Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain;
        
    }
}