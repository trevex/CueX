﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Linq;
using CueX.API;
using CueX.GridSPS;
using CueX.GridSPS.Config;
using CueX.GridSPS.Internal;
using CueX.Numerics;
using CueX.Test.Grains;
using CueX.Test.Helper;
using Orleans;
using Xunit;

namespace CueX.Test
{
    [Collection(ClusterCollection.Name)]
    [TestCaseOrderer("CueX.Test.Helper.PriorityOrderer", "GridSPSTestOrderer")]
    public class GridSPSTest
    {
        private readonly IClusterClient _client;
        private readonly ISpatialPubSub _pubSub;

        public GridSPSTest(ClusterFixture fixture)
        {
            _client = fixture.Client;
            var builder = new GridSpatialPubSubBuilder();
            builder.Configure(config => { config.PartitionSize = 0.2d; });
            _pubSub = builder.Build(_client).GetAwaiter().GetResult();
        }

        [Fact, TestPriority(10)]
        public async void TestGridConfigurationService()
        {
            var config = GridConfiguration.Default();
            config.PartitionSize = 3.0d;
            var configGrain = _client.GetGrain<IGridConfigurationGrain>(GridConfigurationGrain.DefaultKey);
            await configGrain.SetConfiguration(config);
            var savedConfig = await configGrain.GetConfiguration();
            Assert.Equal(savedConfig.PartitionSize, config.PartitionSize);
        }

        [Fact]
        public async void TestGridInsert()
        {
            var spatialGrain = _client.GetGrain<IBasicSpatialGrain>(1);
            await spatialGrain.SetPosition(new Vector3d(1d, 1d, 0d));
            await _pubSub.Insert(spatialGrain);
            var partitionGrain = _client.GetGrain<IGridPartitionGrain>("5,5");
            var children = await partitionGrain.GetChildren();
            Assert.Equal(1, children.Count());
        }
        
        [Fact]
        public async void TestGridRemove()
        { 
            // Insert a spatial grain
            var spatialGrain = _client.GetGrain<IBasicSpatialGrain>(2);
            await spatialGrain.SetPosition(new Vector3d(2d, 2d, 0d));
            await _pubSub.Insert(spatialGrain);
            Assert.Equal(true, await spatialGrain.HasParent());
            // Remove it again
            await _pubSub.Remove(spatialGrain);
            var partitionGrain = _client.GetGrain<IGridPartitionGrain>("10,10");
            var children = await partitionGrain.GetChildren();
            Assert.Equal(0, children.Count());
        }
    }
}