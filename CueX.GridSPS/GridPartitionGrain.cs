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
                State.InterestManager.Initialize(State.Config.PartitionSize, IndexHelper.GetPartitionIndices(this.GetPrimaryKeyString(), State.Config.PartitionSize));
                State.IsInitialized = true;
                await WriteStateAsync();
            }
            await base.OnActivateAsync();
        }
        
        public async Task<bool> HandleSubscription<T>(T subscriber, SubscriptionDetails details) where T : ISpatialGrain
        {
            var eventName = details.EventTypeFilter.GetTypename();
            var filter = new SubscriptionFilter
            {
                Area = details.Area,
                OriginTypeFilter = details.OriginTypeFilter
            };
            // Setup interest management
            var result = State.InterestManager.Add(subscriber, eventName, filter);
            // If the subscription could not be added, return false
            if (!result) return false;
            // Make sure events are forwarded
            var queue = State.InterestManager.GetForwardDelta(subscriber, eventName, filter);
            if (queue.Count > 0) await ProcessForwardCommandQueue(queue);
            await WriteStateAsync();

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
            return Task.FromResult(State.InterestManager.GetPartitionIndices());
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

        private Task ProcessForwardCommandQueue(Queue<ForwardCommand> queue)
        {
            var currentPartitionId = "";
            IGridPartitionGrain currentPartitionGrain = null;
            foreach (var cmd in queue)
            {
                // ForwardCommands for the same partition are in order, therefore cache the current partition
                if (cmd.PartitionId != currentPartitionId || currentPartitionGrain == null)
                {
                    currentPartitionGrain = GrainFactory.GetGrain<IGridPartitionGrain>(cmd.PartitionId);
                    currentPartitionId = cmd.PartitionId;
                }
                // TODO: currentPartitionGrain...
            }
            return Task.CompletedTask;
        }
    }
}