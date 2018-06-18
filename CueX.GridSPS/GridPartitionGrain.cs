// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CueX.Core;
using CueX.Core.Controller;
using CueX.Core.Subscription;
using CueX.Geometry;
using CueX.GridSPS.Config;
using CueX.GridSPS.Controller;
using CueX.GridSPS.Internal;
using Orleans;


namespace CueX.GridSPS
{
    public class GridPartitionGrain : Grain<GridPartitionGrainState>, IGridPartitionGrain
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
                State.ForwardManager.PartitionIndices =
                    IndexHelper.GetPartitionIndices(this.GetPrimaryKeyString(), State.Config.PartitionSize);
                State.IsInitialized = true;
                await WriteStateAsync();
            }
            await base.OnActivateAsync();
        }
        
        public async Task<bool> HandleSubscription<T>(T subscriber, SubscriptionDetails details) where T : ISpatialGrain
        {
            var eventName = details.EventTypeFilter.GetTypename();
            var result = State.InterestManager.Add(subscriber, eventName, new SubscriptionFilter
            {
                Area = details.Area,
                OriginTypeFilter = details.OriginTypeFilter
            });
            // If the subscription could not be added, return false
            if (!result) return false;
            await WriteStateAsync();
            
            // TODO: setup forward tables of neighbours depending on details.Area
            var maxDistance = GetMaxDistance(details);
            // Use direct forwarding table, do not flood, just tell all other partition what you need
            // MaxDistance VS. AdaptIfNecessaryOnMove?
            
            return true;
        }

        public Task HandleSpatialEvent<T>(T eventValue) where T : SpatialEvent
        {
            var eventName = EventHelper.GetEventName<T>();
            var result = State.InterestManager.TryGetEventFilters(eventName, out var eventInterestFilters);
            // If no filters exist, no one subscribed the event
            if (!result) return Task.CompletedTask;
            // Otherwise for each pair<ISpatialGrain, Filter> do:
            foreach (var keyValuePair in eventInterestFilters)
            {
                if (!keyValuePair.Value.IsApplicable(eventValue)) continue;
                // If event is applicable, send it to the grain
                var task = keyValuePair.Key.ReceiveSpatialEvent(eventValue);
                // Do NOT wait, but rather attach an error handling continuation
                task.ContinueWith(t => _logger.LogError(t.Exception.ToString()), TaskContinuationOptions.OnlyOnFaulted);
            }
            // TODO: forward
            return Task.CompletedTask;
        }

        public Task<int> GetInterestCount()
        {
            return Task.FromResult(State.InterestManager.GetInterestCount());
        }

        public Task<Tuple<int, int>> GetPartitionIndices()
        {
            return Task.FromResult(State.ForwardManager.PartitionIndices);
        }

        private double GetMaxDistance(SubscriptionDetails details)
        {
            return State.Config.PartitionHalfDiagonal + details.Area.GetHalfBoundingBoxWidth();
        }
        
        public async Task Add<T>(T spatialGrain) where T : ISpatialGrain
        {
            State.Children.Add(spatialGrain);
            await spatialGrain.SetController(new GridController());
            await spatialGrain.ReceiveControlEvent(new SetParentEvent{ Partition =  this.AsReference<IGridPartitionGrain>() });
            await WriteStateAsync();
        } 
        
        public async Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain
        {
            // TODO: remove subscriptions!
            var found = State.Children.Remove(spatialGrain);
            if (found) await WriteStateAsync();
            return found;
        }

        public Task<IEnumerable<ISpatialGrain>> GetChildren()
        {
            return Task.FromResult(State.Children.AsEnumerable());
        }

    }
}