// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using System.Threading.Tasks;
using CueX.API;
using CueX.Core;
using CueX.GridSPS.Config;
using CueX.GridSPS.Internal;
using Orleans;

namespace CueX.GridSPS
{
    /// <summary>
    /// Builder class to help configure and bootstrap a ISpatialPubSub using the grid-based implementation.
    /// </summary>
    public class GridSpatialPubSubBuilder : SpatialPubSubBuilder
    {
        private GridConfiguration _config =  GridConfiguration.Default();

        public GridSpatialPubSubBuilder Configure(Action<GridConfiguration> configure)
        {
            configure(_config);
            return this;
        }

        public GridSpatialPubSubBuilder UsePartitionSize(double size)
        {
            _config.PartitionSize = size;
            return this;
        }
        
        public override async Task<ISpatialPubSub> Build(IClusterClient client)
        {
            var pubSub = new GridSpatialPubSub(client, _config);
            await pubSub.Initialize();
            return pubSub;
        }
    }
}