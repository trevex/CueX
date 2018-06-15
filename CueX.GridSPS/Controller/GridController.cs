// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Controller;
using CueX.Core.Subscription;
using Orleans;

namespace CueX.GridSPS.Controller
{
    public class GridController : IController
    {
        private IGridPartitionGrain _partition;
 
        public Task ReceiveControlEvent<TGrainInterface, TState, TEvent>(SpatialGrain<TGrainInterface, TState> spatialGrain, TEvent controlEvent) where TGrainInterface : ISpatialGrain where TState : SpatialGrainState, new() where TEvent : ControlEvent
        {
            if (typeof(TEvent) == typeof(SetParentEvent))
            {
                var e = controlEvent as SetParentEvent;
                _partition = e.Partition;
            }
            else
            {
                throw new System.NotImplementedException();
            }
            return Task.CompletedTask;
        }

        public Task<bool> HandleSubscription<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain, SubscriptionDetails details) where TGrainInterface : ISpatialGrain where TState : SpatialGrainState, new()
        {
            return _partition.HandleSubscription(spatialGrain.AsReference<TGrainInterface>(), details);
        }

        public Task Initialize<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain) where TGrainInterface : ISpatialGrain where TState : SpatialGrainState, new()
        {
            return Task.CompletedTask;
        }

        public Task<bool> Destroy<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain) where TGrainInterface : ISpatialGrain where TState : SpatialGrainState, new()
        {
            // Make sure child is removed from partition
            return _partition.Remove(spatialGrain.AsReference<TGrainInterface>());
        }
        
        public Task<bool> HasPartition()
        {
            return Task.FromResult(_partition != null);
        }
    }
}