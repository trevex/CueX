// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;


namespace CueX.GridSPS
{
    public class GridPartitionGrain : PartitionGrain<IGridPartitionGrain, GridPartitionGrainState>, IGridPartitionGrain
    {
        private readonly ILogger _logger;
        private readonly IGridConfigurationService _configService;
        
        public GridPartitionGrain(ILogger<GridPartitionGrain> logger, IGridConfigurationService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        public override async Task OnActivateAsync()
        {
            // Check if this partition was already initialized, if not prepare this partition for usage 
            if (!State.IsInitialized)
            {
                // Acquire configuration from config service
                State.Config = await _configService.GetConfiguration();
                State.InterestFilterMap = new Dictionary<string, Dictionary<ISpatialGrain, SubscriptionFilter>>();
                State.GrainInterestMap = new Dictionary<ISpatialGrain, List<string>>();
                State.ForwardMap = new Dictionary<string, List<IGridPartitionGrain>>();
                State.IsInitialized = true;
                await WriteStateAsync();
            }
            await base.OnActivateAsync();
        }
        
        public override async Task<bool> HandleSubscription<T>(T subscribingGrain, SubscriptionDetails details)
        {
            var eventType = details.EventFilter.GetTypeString();
            var result = State.InterestFilterMap.TryGetValue(eventType, out var eventInterestFilters);
            if (!result)
            {
                eventInterestFilters = new Dictionary<ISpatialGrain, SubscriptionFilter>();
                State.InterestFilterMap[eventType] = eventInterestFilters;
            }
            else if (eventInterestFilters.ContainsKey(subscribingGrain))
            {
                return false;
            }
            
            eventInterestFilters[subscribingGrain] = new SubscriptionFilter
            {
                Area = details.Area,
                OriginFilter = details.OriginFilter
            };
            result = State.GrainInterestMap.TryGetValue(subscribingGrain, out var grainInterests);
            if (!result)
            {
                grainInterests = new List<string>();
                State.GrainInterestMap[subscribingGrain] = grainInterests;
            } 
            grainInterests.Add(eventType);

            Console.WriteLine("1"); // TODO: remove
            
            await WriteStateAsync();
            
            // TODO: setup forward tables of neighbours depending on details.Area
            
            Console.WriteLine("2"); // TODO: remove
            
            return true;
        }

        public Task<int> GetInterestCount()
        {
            var count = 0;
            foreach (var grainInterestsPair in State.GrainInterestMap)
            {
                count += grainInterestsPair.Value.Count;
            }
            return Task.FromResult(count);
        }
    }
}