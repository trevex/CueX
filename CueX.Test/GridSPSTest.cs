// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using CueX.API;
using CueX.GridSPS;
using CueX.GridSPS.Config;
using CueX.Test.Helper;
using Orleans;
using Xunit;

namespace CueX.Test
{
    [Collection(ClusterCollection.Name)]
    public class GridSPSTest
    {
        private readonly IClusterClient _client;
        private readonly ISpatialPubSub _pubSub;

        public GridSPSTest(ClusterFixture fixture)
        {
            _client = fixture.Client;
            var builder = new GridSpatialPubSubBuilder();
            builder.Configure(config =>
            {
                // config.something = somethingelse
            });
            _pubSub = builder.Build(_client).GetAwaiter().GetResult();
        }

        [Fact]
        public async void TestGridConfigurationService()
        {
            var config = GridConfiguration.Default();
            config.PartitionSize = 2.0d;
            var configGrain = _client.GetGrain<IGridConfigurationGrain>(GridConfigurationGrain.DefaultKey);
            await configGrain.SetConfiguration(config);
            var savedConfig = await configGrain.GetConfiguration();
            Assert.Equal(config.PartitionSize, savedConfig.PartitionSize);
        }
    }
}