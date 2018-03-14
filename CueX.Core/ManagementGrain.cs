// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.API;
using Orleans;

namespace CueX.Core
{
    public abstract class ManagementGrain<TState> : Grain<TState>, IManagementGrain
        where TState : ManagementGrainState, new()
    {
        public abstract Task Insert<T>(T spatialGrain) where T : ISpatialGrain;
        public abstract Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain;
    }
}