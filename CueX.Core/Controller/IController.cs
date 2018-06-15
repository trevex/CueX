// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core.Subscription;

namespace CueX.Core.Controller
{
    public interface IController
    {
        Task ReceiveControlEvent<TGrainInterface, TState, TEvent>(SpatialGrain<TGrainInterface, TState> spatialGrain, TEvent controlEvent)
            where TEvent : ControlEvent where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain;
        Task<bool> HandleSubscription<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain, SubscriptionDetails details) 
            where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain;
        Task Initialize<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain) 
            where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain;
        Task<bool> Destroy<TGrainInterface, TState>(SpatialGrain<TGrainInterface, TState> spatialGrain) 
            where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain;
    }
}