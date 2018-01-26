using System;
using System.Threading.Tasks;
using CueX.Numerics;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime.Configuration;
using SimpleExample.Grains;

namespace SimpleExample.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var client = await ConnectToLocalhostSilo();
            var someGrain = client.GetGrain<ISimpleGrain>(0);
            await someGrain.SetPosition(Vector3d.One());
            Console.WriteLine(await someGrain.GetPosition());
        }

        private static async Task<IClusterClient> ConnectToLocalhostSilo()
        {
            var config = ClientConfiguration.LocalhostSilo();
            var builder = new ClientBuilder()
                .UseConfiguration(config)
                // Add grain assemblies
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ISimpleGrain).Assembly))
                .ConfigureLogging(logging => logging.AddConsole());
            var client = builder.Build();
            await client.Connect();
            return client;
        }
    }
}