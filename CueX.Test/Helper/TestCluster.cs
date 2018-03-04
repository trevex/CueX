using System;
using System.Net;
using System.Threading.Tasks;
using CueX.Core;
using CueX.GridSPS;
using CueX.Test.Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

namespace CueX.Test.Helper
{
    public class TestCluster : IDisposable
    {
        private const int SiloPort = 11111;
        private const int GatewayPort = 30000;

        public readonly ISiloHost Silo;
        public readonly IClusterClient Client;
        
        public TestCluster()
        {
            Silo = StartSilo().GetAwaiter().GetResult();
            Client = StartClient().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Client.Close().GetAwaiter().GetResult();
            Silo.StopAsync().GetAwaiter().GetResult();
        }
        
        private static async Task<ISiloHost> StartSilo()
        {
            // run some hardware checks (vector instruction support)
            SpatialPubSubConfigurationHelper.CheckHardwareSupport();
            // Default values
            var siloAddress = IPAddress.Loopback;
            // Configure localhost silo
            var builder = new SiloHostBuilder()
                .Configure(options => { options.ClusterId = "LocalCueXCluster"; })
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, SiloPort))
                .ConfigureEndpoints(siloAddress, SiloPort, GatewayPort)
                .ConfigureApplicationParts(AddHostApplicationParts)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    // logging.SetMinimumLevel(LogLevel.Debug);
                })
                .AddMemoryGrainStorageAsDefault();
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
        
        private static async Task<IClusterClient> StartClient()
        {
            var siloAddress = IPAddress.Loopback;
            IClusterClient client = new ClientBuilder()
                .ConfigureCluster(options => options.ClusterId = "helloworldcluster")
                .UseStaticClustering(options => options.Gateways.Add((new IPEndPoint(siloAddress, GatewayPort)).ToGatewayUri()))
                .ConfigureApplicationParts(AddClientApplicationParts)
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            return client;
        }
        
        private static void AddHostApplicationParts(IApplicationPartManager parts)
        {
            GridConfigurationHelper.AddGridHostApplicationParts(parts);
            parts.AddApplicationPart(typeof(BasicSpatialGrain).Assembly).WithReferences();
        }
        
        private static void AddClientApplicationParts(IApplicationPartManager parts)
        {
            GridConfigurationHelper.AddGridClientApplicationParts(parts);
            parts.AddApplicationPart(typeof(IBasicSpatialGrain).Assembly);
        }

    }
}