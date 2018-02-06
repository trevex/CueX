using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CueX.Core;
using CueX.GridSPS;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
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
            // run some hardware checks (vector instruction support)
            SpatialPubSubConfigurationHelper.CheckHardwareSupport();
            // define the cluster configuration (temporarily required in the beta version,
            // will not be required by the final release)
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            // add providers to the legacy configuration object.
            config.AddMemoryStorageProvider();
            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                // Add grain assemblies
                .ConfigureApplicationParts(parts =>
                {
                    GridConfigurationHelper.AddGridHostApplicationParts(parts);
                    AddExampleHostApplicationParts(parts);
                })
                // Logging setup
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    // logging.SetMinimumLevel(LogLevel.Debug);
                });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private static void AddExampleHostApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(SimpleGrain).Assembly)
                .WithReferences();
        }
    }
}