// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CueX.Core;


namespace CueX.GridSPS
{
    public class GridPartitionGrain : PartitionGrain<GridPartitionGrainState>, IGridPartitionGrain
    {
        private readonly ILogger _logger;
        
        public GridPartitionGrain(ILogger<GridPartitionGrain> logger)
        {
            _logger = logger;
        }

        
        public override Task Add<T>(T spatialGrain)
        {
            State.Children.Add(spatialGrain);
            return WriteStateAsync();
        }

        public override async Task<bool> Remove<T>(T spatialGrain)
        {
            var found = State.Children.Remove(spatialGrain);
            await WriteStateAsync();
            return found;
        }

        public override Task OnActivateAsync()
        {
            // Check if this partition was already initialized, if not prepare this partition for usage 
            if (!State.IsInitialized)
            {
                // TODO: needs to do some prep stuff?
            }
            return base.OnActivateAsync();
        }
    }
}