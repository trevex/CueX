// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.API;
using Orleans;

namespace CueX.GridSPS
{
    public class GridPartitionGrain : IPartitionGrain, IGrainWithIntegerKey
    {
        public Task Add<T>(T spatialGrain) where T : ISpatialGrain
        {
            return Task.CompletedTask;
        }

        public Task Remove<T>(T spatialGrain) where T : ISpatialGrain
        {
            return Task.CompletedTask;
        }
    }
}