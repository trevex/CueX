using System;
using System.Threading.Tasks;
using CueX.GridSPS;
using CueX.Numerics;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Runtime.Configuration;
using SimpleExample.Grains;

namespace SimpleExample.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var client = await ConnectToLocalhostSilo();
            var simpleGrain = client.GetGrain<ISimpleGrain>(0);
            await simpleGrain.SetPosition(Vector3d.One());
            Console.WriteLine(await simpleGrain.GetPosition());
            var gridPartitionGrain = client.GetGrain<IGridPartitionGrain>(0);
            await gridPartitionGrain.Add(simpleGrain);
            Console.WriteLine(await gridPartitionGrain.Remove(simpleGrain));
        }

        private static async Task<IClusterClient> ConnectToLocalhostSilo()
        {
            var config = ClientConfiguration.LocalhostSilo();
            var builder = new ClientBuilder()
                .UseConfiguration(config)
                // Add grain assemblies
                .ConfigureApplicationParts(parts =>
                {
                    GridConfigurationHelper.AddGridClientApplicationParts(parts);
                    AddExampleApplicationParts(parts);
                })
                .ConfigureLogging(logging => logging.AddConsole());
            var client = builder.Build();
            await client.Connect();
            return client;
        }

        private static void AddExampleApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(ISimpleGrain).Assembly);
        }
    }
}