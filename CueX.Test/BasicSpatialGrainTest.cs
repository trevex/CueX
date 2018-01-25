// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using CueX.Numerics;
using CueX.Test.Grains;
using CueX.Test.Helper;
using Orleans.TestKit;
using Xunit;

namespace CueX.Test
{
    [Collection(SiloCollection.Name)]
    public class BasicSpatialGrainTest
    {
        private readonly TestKitSilo _silo;

        public BasicSpatialGrainTest(SiloFixture fixture)
        {
            _silo = fixture.Silo;
        }

        [Fact]
        public async void TestPositionAssignment()
        {
            long id = new Random().Next();
            IBasicSpatialGrain spatialGrain = _silo.CreateGrain<BasicSpatialGrain>(0);

            var pos = new Vector3d(1.0, 2.0, 3.0);
            await spatialGrain.SetPosition(pos);
            var assignedPos = await spatialGrain.GetPosition();
            Assert.Equal(true, pos.Equals(assignedPos));
        }
    }
}