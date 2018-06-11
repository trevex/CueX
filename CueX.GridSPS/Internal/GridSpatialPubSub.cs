// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;
using Orleans;

namespace CueX.GridSPS.Internal
{
    internal class GridSpatialPubSub : ISpatialPubSub
    {
        private readonly IClusterClient _client;
        private readonly GridConfiguration _config;
        
        public GridSpatialPubSub(IClusterClient client, GridConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public Task Initialize()
        {
            var configGrain = _client.GetGrain<IGridConfigurationGrain>(GridConfigurationGrain.DefaultKey);
            return configGrain.SetConfiguration(_config);
        }

        public Task Insert<T>(T spatialGrain) where T : ISpatialGrain
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain
        {
            throw new NotImplementedException();
        }

        public async Task Dispatch<T>(T spatialEvent) where T : SpatialEvent
        {
            // TODO:
        }
    }
}