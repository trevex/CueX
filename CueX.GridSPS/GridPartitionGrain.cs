// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.API;
using CueX.Core;
using Orleans;

namespace CueX.GridSPS
{
    public class GridPartitionGrain : PartitionGrain<GridPartitionGrainState>, IGridPartitionGrain
    {
        public override Task Add<T>(T spatialGrain)
        {
            return Task.CompletedTask;
        }

        public override Task Remove<T>(T spatialGrain)
        {
            return Task.CompletedTask;
        }
    }
}