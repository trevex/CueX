// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
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
                State.IsInitialized = true;
                await WriteStateAsync();
            }
            await base.OnActivateAsync();
        }
        
        public override async Task<bool> HandleSubscription<T>(T subscribingGrain, SubscriptionDetails details)
        {
            var result = State.InterestFilterMap.TryGetValue(details.EventTypeName, out var eventInterestFilters);
            if (!result)
            {
                eventInterestFilters = new Dictionary<ISpatialGrain, SubscriptionFilter>();
                State.InterestFilterMap[details.EventTypeName] = eventInterestFilters;
            }
            else if (eventInterestFilters.ContainsKey(subscribingGrain))
            {
                return false;
            }
            
            eventInterestFilters[subscribingGrain] = new SubscriptionFilter
            {
                Area = details.Area
            };
            result = State.GrainInterestMap.TryGetValue(subscribingGrain, out var grainInterests);
            if (!result)
            {
                grainInterests = new List<string>();
                State.GrainInterestMap[subscribingGrain] = grainInterests;
            } 
            grainInterests.Add(details.EventTypeName);
            
            await WriteStateAsync();
            
            // TODO: setup forward tables of neighbours depending on details.Area
            
            return true;
        }

        public override Task HandleEvent(string eventName, SpatialEvent eventValue)
        {
            var result = State.InterestFilterMap.TryGetValue(eventName, out var eventInterestFilters);
            if (result) {
                foreach (var keyValuePair in eventInterestFilters)
                {
                    
                    if (keyValuePair.Value.IsApplicable(eventValue))
                    {
                        keyValuePair.Key.ReceiveEvent(eventName, eventValue);
                    }
                }
            }
            // TODO: forward
            return Task.CompletedTask;
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