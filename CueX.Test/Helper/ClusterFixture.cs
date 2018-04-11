// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using CueX.GridSPS.Config;
using CueX.Test.Grains;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.TestingHost;
using Orleans.Hosting;

namespace CueX.Test.Helper
{
    public class ClusterFixture : IDisposable
    {
        public ClusterFixture()
        {
            var builder = new TestClusterBuilder();
            builder.Options.ClusterId = "CueX.TestCluster";
            builder.AddClientBuilderConfigurator<TestBuilderConfigurator>();
            builder.AddSiloBuilderConfigurator<TestBuilderConfigurator>();
            Cluster = builder.Build();
            Cluster.Deploy();
            Client = Cluster.Client;
        }

        public void Dispose()
        {
            Client.Dispose();
            Cluster.StopAllSilos();
        }

        public TestCluster Cluster { get; private set; }
        public IClusterClient Client { get; private set; }

        private class TestBuilderConfigurator : IClientBuilderConfigurator, ISiloBuilderConfigurator
        {
            public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
            {
                clientBuilder.ConfigureApplicationParts(parts =>
                {
                    GridConfigurationHelper.AddClientApplicationParts(parts);
                    parts.AddApplicationPart(typeof(IBasicSpatialGrain).Assembly);
                });
            }

            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder.ConfigureApplicationParts(parts =>
                {
                    GridConfigurationHelper.AddSiloApplicationParts(parts);
                    parts.AddApplicationPart(typeof(BasicSpatialGrain).Assembly).WithReferences();
                });
                hostBuilder.AddMemoryGrainStorageAsDefault();
                hostBuilder.ConfigureServices(GridConfigurationHelper.AddServices);
            }
        }
    }
}