// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;
using CueX.Numerics;
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

        public async Task Insert<T>(T spatialGrain) where T : ISpatialGrain
        {
            await InsertAt(spatialGrain, await spatialGrain.GetPosition());
        }

        private async Task InsertAt<T>(T spatialGrain, Vector3d position) where T : ISpatialGrain
        {
            var partitionKey = IndexHelper.GetPartitionKeyForPosition(position, _config.PartitionSize);
            var partition = _client.GetGrain<IGridPartitionGrain>(partitionKey);
            await partition.Add(spatialGrain);
        }

        public async Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain
        {
            return await spatialGrain.RemoveSelfFromParent();
        }

        public async Task Dispatch<T>(T spatialEvent) where T : SpatialEvent
        {
            var partitionKey = IndexHelper.GetPartitionKeyForPosition(spatialEvent.Position, _config.PartitionSize);
            var partition = _client.GetGrain<IGridPartitionGrain>(partitionKey);
            await partition.HandleEvent(EventHelper.GetEventName<T>(), spatialEvent);
        }
    }
}