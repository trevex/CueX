// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using CueX.Numerics;
using CueX.Test.Grains;
using CueX.Test.Helper;
using Orleans;
using Xunit;

namespace CueX.Test
{
    [Collection(ClusterCollection.Name)]
    public class BasicSpatialGrainTest
    {
        private readonly IClusterClient _client;

        public BasicSpatialGrainTest(ClusterFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async void TestPositionAssignment()
        {
            long id = new Random().Next();
            var spatialGrain = _client.GetGrain<IBasicSpatialGrain>(0);
            var pos = new Vector3d(1.0, 2.0, 3.0);
            await spatialGrain.SetPosition(pos);
            var assignedPos = await spatialGrain.GetPosition();
            Assert.Equal(true, pos.Equals(assignedPos));
        }
    }
}