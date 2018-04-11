﻿using System;
using System.Net;
using System.Threading.Tasks;
using CueX.GridSPS;
using CueX.GridSPS.Config;
using CueX.Numerics;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;
using Orleans.Runtime;
using SimpleExample.Grains;

namespace SimpleExample.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var client = await StartClientWithRetries();
            var simpleGrain = client.GetGrain<ISimpleGrain>(0);
            await simpleGrain.SetPosition(Vector3d.One());
            Console.WriteLine(await simpleGrain.GetPosition());
            var gridPartitionGrain = client.GetGrain<IGridPartitionGrain>("1");
            await gridPartitionGrain.Add(simpleGrain);
            Console.WriteLine(await gridPartitionGrain.Remove(simpleGrain));
        }

        private static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var siloAddress = IPAddress.Loopback;
                    var gatewayPort = 30000;
                    client = new ClientBuilder()
                        .UseLocalhostClustering()
                        .ConfigureApplicationParts(AddClientApplicationParts)
                        .ConfigureLogging(logging => logging.AddConsole())
                        .ConfigureServices(GridConfigurationHelper.AddServices)
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