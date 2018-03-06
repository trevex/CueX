using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CueX.Core;
using CueX.GridSPS;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using SimpleExample.Grains;

namespace LocalhostSilo
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();
                await host.StopAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // Default values
            int siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;
            // Configure silo
            var builder = new SiloHostBuilder()
                .Configure(options => { options.ClusterId = "LocalCueXCluster"; })
                // Setup silo on localhost and use development cluster 
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                // Add grain assemblies
                .ConfigureApplicationParts(AddHostApplicationParts)
                // Logging setup
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
        
        private static void AddHostApplicationParts(IApplicationPartManager parts)
        {
            parts.AddFromAppDomain(); // Loads Orleans-specific assemblies 
            GridConfigurationHelper.AddGridHostApplicationParts(parts);
            parts.AddApplicationPart(typeof(SimpleGrain).Assembly).WithReferences();
        }

    }
}