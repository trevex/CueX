// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CueX.API;
using Orleans;

namespace CueX.Core
{
    /// <summary>
    /// Abstract class providing the basic functionality necessary for sub-classes
    /// to manage a partition of the SPS area.
    /// </summary>
    /// <typeparam name="TState">Application-specific state data type, that also holds <see cref="T:CueX.Core.PartitionGrainState" />.</typeparam>
    /// <typeparam name="TGrainInterface"></typeparam>
    public abstract class PartitionGrain<TGrainInterface, TState> : Grain<TState>, IPartitionGrain
        where TState : PartitionGrainState, new() where TGrainInterface : IPartitionGrain
    {

        public async Task Add<T>(T spatialGrain) where T : ISpatialGrain
        {
            State.Children.Add(spatialGrain);
            await spatialGrain.SetParent(this.AsReference<TGrainInterface>());
            await WriteStateAsync();
        } 
        
        public async Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain
        {
            var found = State.Children.Remove(spatialGrain);
            if (found) await WriteStateAsync();
            return found;
        }

        public Task<IEnumerable<ISpatialGrain>> GetChildren()
        {
            return Task.FromResult(State.Children.AsEnumerable());
        }
    }
}