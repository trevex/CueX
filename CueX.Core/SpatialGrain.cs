// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CueX.Core.Exception;
using CueX.Core.Stream;
using CueX.Core.Subscription;
using CueX.Geometry;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;

namespace CueX.Core
{
    /// <summary>
    /// Abstract class providing the basic functionality necessary for sub-classes
    /// to be insertable into the PubSub-System.
    /// </summary>
    /// <typeparam name="TState">Application-specific state data type, that also holds <see cref="SpatialGrainState"/>.</typeparam>
    /// <typeparam name="TGrainInterface"></typeparam>
    public abstract class SpatialGrain<TGrainInterface, TState> : Grain<TState>, ISpatialGrain, ISubscriptionSubject
        where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain
    {
        private readonly Dictionary<string, Func<SpatialEvent, Task>> _callbacks = new Dictionary<string, Func<SpatialEvent, Task>>();
        private readonly Dictionary<Type /* EventType */, List<IAnyHandle>> _subscriptionHandles = new Dictionary<Type, List<IAnyHandle>>();
        private readonly IControlService _controlService;
        private readonly ILogger _logger;

        public SpatialGrain(ILogger<SpatialGrain<TGrainInterface, TState>> logger, IControlService controlService)
        {
            _logger = logger;
            _controlService = controlService;
        }
        
        public override async Task OnActivateAsync()
        {
            if (!State.IsInitialized)
            {
                State.Controller = await _controlService.GetSpatialGrainController();
                State.IsInitialized = true;
                await WriteStateAsync();
            }
            RecompileCallbacks();
            ResumeSubscriptions();
            await base.OnActivateAsync();
        }

        public async Task SetPosition(Vector3d newPosition)
        {
            State.Position = newPosition;
            await WriteStateAsync();
        }

        public Task<Vector3d> GetPosition()
        {
            return Task.FromResult(State.Position);
        }

        public Task ReceiveSpatialEvent<T>(T spatialEvent) where T : SpatialEvent
        {
            _callbacks[EventHelper.GetEventName<T>()](spatialEvent);
            return Task.CompletedTask;
        }

        public Task ReceiveControlEvent<T>(T logicEvent) where T : IControlEvent
        {
            return State.Controller.ReceiveControlEvent(logicEvent);
        }

        public async Task<bool> SubscribeWithDetails<T>(SubscriptionDetails details, Func<T, Task> callback) where T : SpatialEvent
        {
            var method = callback.Method;
            if (method.DeclaringType == null || !CodeGenerator.IsValidLanguageIndependentIdentifier(method.Name))
            {   
                throw new SubscriptionCallbackNotMethodException();
            }
            var self = GetType();
            if (self.GetMethod(method.Name) == null)
            {
                throw new SubscriptionCallbackNotMemberException();
            }
            // save callback for direct invocation via `ReceiveSpatialEvent`
            var eventName = EventHelper.GetEventName<T>();
            _callbacks[eventName] = e => callback((T) e); 
            // save reflection info to reconstruct callbacks during activation
            var eventType = typeof(T);
            State.CallbackMethodInfos[eventType] = callback.Method;
            // subscribe to the event
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            var ids = await State.Controller.GetStreamIdsForNewSubscription(details);
            // if there is no list already create one
            if (!_subscriptionHandles.ContainsKey(eventType)) _subscriptionHandles[eventType] = new List<IAnyHandle>();
            // subscribe to all streams
            foreach (var id in ids)
            {
                var stream = streamProvider.GetStream<T>(id, Constants.StreamNamespace);
                var handle = await stream.SubscribeAsync(new StreamObserver<T>(_logger, callback));
                // add handle to be able to unsubscribe
                _subscriptionHandles[eventType].Add(new AnyHandle<T>(handle));
                // remember which stream guid belongs to what type of event
                State.StreamAssociation[eventName] = id;
            }
            await WriteStateAsync();
            return true;
        }

        protected SubscriptionBuilder<T> SubscribeTo<T>() where T : SpatialEvent
        {
            return new SubscriptionBuilder<T>(this);
        }

        private void RecompileCallbacks()
        {
            foreach (var pair in State.CallbackMethodInfos)
            { 
                var methodInfo = pair.Value;
                var targetType = pair.Key;
                var interfaceParam = Expression.Parameter(typeof(SpatialEvent));
                var eventParam = Expression.Convert(interfaceParam, targetType);
                var lambdaExpression = Expression.Lambda<Func<SpatialEvent, Task>>(
                    Expression.Call(Expression.Constant(this), methodInfo, eventParam), interfaceParam);
                _callbacks[targetType.ToString()] = lambdaExpression.Compile();
            }
        }

        private async void ResumeSubscriptions()
        {
            var streamProvider = GetStreamProvider(Constants.StreamProviderName);
            var methodInfo = GetType().GetMethod("ResumeSubscriptionFor");
            foreach (var pair in State.CallbackMethodInfos)
            {
                var targetType = pair.Key;
                var resumeRef = methodInfo.MakeGenericMethod(targetType);
                await (Task)resumeRef.Invoke(this, new object[] { streamProvider });
            }  
        }

        private async Task ResumeSubscriptionFor<T>(IStreamProvider streamProvider) where T : SpatialEvent
        {
            var eventName = EventHelper.GetEventName<T>();
            var eventType = typeof(T);
            var stream = streamProvider.GetStream<T>(State.StreamAssociation[eventName], Constants.StreamNamespace);
            var subscriptionHandles = await stream.GetAllSubscriptionHandles();
            if (subscriptionHandles == null || subscriptionHandles.Count == 0) return;
            // if there is no list already create one
            if (!_subscriptionHandles.ContainsKey(eventType)) _subscriptionHandles[eventType] = new List<IAnyHandle>();
            foreach (var handle in subscriptionHandles)
            {
                await handle.ResumeAsync(new StreamObserver<T>(_logger, _callbacks[eventName]));
                _subscriptionHandles[eventType].Add(new AnyHandle<T>(handle));
            } 
        }
        

    }
}