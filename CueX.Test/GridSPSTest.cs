﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS;
using CueX.GridSPS.Config;
using CueX.Geometry;
using CueX.Test.Events;
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
            await configGrain.SetController();
            var savedConfig = await configGrain.GetConfiguration();
            var savedController = await configGrain.GetController();
            Assert.Equal(savedConfig.PartitionSize, config.PartitionSize);
            Assert.NotNull(savedController);
        }

        [Fact]
        public async void TestGridInsert()
        {
            var spatialGrain = _client.GetGrain<ITestSpatialGrain>(1);
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
            var spatialGrain = _client.GetGrain<ITestSpatialGrain>(2);
            await spatialGrain.SetPosition(new Vector3d(2d, 2d, 0d));
            await _pubSub.Insert(spatialGrain);
            Assert.Equal(true, await spatialGrain.HasGridPartition());
            // Remove it again
            await _pubSub.Remove(spatialGrain);
            var partitionGrain = _client.GetGrain<IGridPartitionGrain>("10,10");
            var children = await partitionGrain.GetChildren();
            Assert.Equal(0, children.Count());
        }
        
        [Fact]
        public async void TestGridSubscription()
        { 
            var spatialGrain = _client.GetGrain<ITestSpatialGrain>(3);
            await spatialGrain.SetPosition(new Vector3d(3d, 3d, 0d));
            await _pubSub.Insert(spatialGrain);
            var partitionGrain = _client.GetGrain<IGridPartitionGrain>("15,15");
            await spatialGrain.SubscribeToTestEvent();
            Assert.Equal(1, await partitionGrain.GetInterestCount());
            // Publish event to grain
            await spatialGrain.ReceiveSpatialEvent(new TestEvent {Value = "HELLO"});
            Assert.Equal("HELLO", await spatialGrain.GetLastTestEventValue());
            // Recompile callbacks
            await spatialGrain.ForcefullyRecompileCallback();
            // Try again
            await spatialGrain.ReceiveSpatialEvent(new TestEvent {Value = "WORLD"});
            Assert.Equal("WORLD", await spatialGrain.GetLastTestEventValue());
            await _pubSub.Dispatch(new TestEvent {Position = new Vector3d(3d, 3d, 0d), Value = "DISPATCHED"});
            Thread.Sleep(2000); // NOTE: internal event dispatch is async
            Assert.Equal("DISPATCHED", await spatialGrain.GetLastTestEventValue());
        }
        
        [Fact]
        public async void TestPartitionIndices()
        { 
            var partitionGrain = _client.GetGrain<IGridPartitionGrain>("10,10");
            var partitionIndices = await partitionGrain.GetPartitionIndices();
            Assert.Equal(new Tuple<int, int>(10, 10), partitionIndices);
        }
    }
}