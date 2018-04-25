using System;
using System.Threading.Tasks;
using CueX.Core;
using CueX.GridSPS;
using CueX.GridSPS.Config;
using CueX.Numerics;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Hosting;
using Orleans.Runtime;
using SimpleExample.Grains;

namespace SimpleExample.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var client = await StartClientWithRetries();
            var builder = new GridSpatialPubSubBuilder();
            builder.Configure(config => { config.PartitionSize = 0.2d; });
            var pubSub = builder.Build(client).GetAwaiter().GetResult();
            var spatialGrain = client.GetGrain<ISimpleGrain>(0);
            await spatialGrain.SetPosition(new Vector3d(1d, 1d, 0d));
            await pubSub.Insert(spatialGrain);
            await spatialGrain.SubscribeToSimpleEvent();
            Console.WriteLine("All done!");
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .ConfigureApplicationParts(AddClientApplicationParts)
                        .ConfigureLogging(logging => logging.AddConsole())
                        .ConfigureServices(GridConfigurationHelper.AddServices)
                        .AddSimpleMessageStreamProvider(Constants.StreamProviderName)
                        .Build();

                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }
        
        private static void AddClientApplicationParts(IApplicationPartManager parts)
        {
            GridConfigurationHelper.AddClientApplicationParts(parts);
            parts.AddApplicationPart(typeof(ISimpleGrain).Assembly);
        }

    }
}