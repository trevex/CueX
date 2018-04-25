using System;
using System.Net;
using System.Threading.Tasks;
using CueX.Core;
using CueX.GridSPS.Config;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Configuration;
using Orleans.Hosting;
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
            // Configure silo
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                // Add grain assemblies
                .ConfigureApplicationParts(AddHostApplicationParts)
                // Logging setup
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    // logging.SetMinimumLevel(LogLevel.Debug);
                })
                .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                .ConfigureServices(GridConfigurationHelper.AddServices)
                .AddMemoryGrainStorageAsDefault();
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
        
        private static void AddHostApplicationParts(IApplicationPartManager parts)
        { 
            GridConfigurationHelper.AddSiloApplicationParts(parts);
            parts.AddApplicationPart(typeof(SimpleGrain).Assembly).WithReferences();
        }

    }
}