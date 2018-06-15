// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CueX.Core.Controller;
using CueX.Core.Exception;
using CueX.Core.Subscription;
using CueX.Geometry;
using Orleans;

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

        public override Task OnActivateAsync()
        {
            RecompileCallbacksIfNecessary();
            return base.OnActivateAsync();
        }

        /// <summary>
        /// This method recompiles callbacks if they are not in sync with the grain state (potentially dangerous!)
        /// </summary>
        protected void RecompileCallbacksIfNecessary()
        {
            if (_callbacks.Count != State.CallbackMethodInfos.Count)
            {
                foreach (var pair in State.CallbackMethodInfos)
                { 
                    var methodInfo = pair.Value;
                    var targetType = pair.Key;
                    var interfaceParam = Expression.Parameter(typeof(SpatialEvent));
                    var eventParam = Expression.Convert(interfaceParam, targetType);
                    var lambdaExpression = Expression.Lambda<Func<SpatialEvent, Task>>(
                        Expression.Call(Expression.Constant(this), methodInfo, eventParam), interfaceParam);
                    _callbacks[targetType.ToString()] = lambdaExpression.Compile(); // TODO: pair.Key.ToString() is redundant, either clean up EventHelper or this bit
                }
            }
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

        public async Task SetParent<T>(T parent) where T : IPartitionGrain
        {
            State.Parent = parent;
            await WriteStateAsync();
        }

        public Task<bool> RemoveSelfFromParent()
        {
            return State.Parent.Remove(this.AsReference<TGrainInterface>());
        }

        public async Task<bool> SubscribeWithDetails<T>(SubscriptionDetails details, Func<T, Task> callback) where T : SpatialEvent
        {
            var result = await State.Parent.HandleSubscription(this.AsReference<TGrainInterface>(), details);
            if (!result) return false;
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
            _callbacks[EventHelper.GetEventName<T>()] = e => callback((T) e); // TODO: since event name and event type is a strict relation, see if all checks are disabled
            State.CallbackMethodInfos[typeof(T)] = callback.Method; // save reflection info to reconstruct callbacks during activation
            await WriteStateAsync();
            return true;
        }

        public SubscriptionBuilder<T> SubscribeTo<T>() where T : SpatialEvent
        {
            return new SubscriptionBuilder<T>(this);
        }

        public Task ReceiveSpatialEvent<T>(T spatialEvent) where T : SpatialEvent
        {
            _callbacks[EventHelper.GetEventName<T>()](spatialEvent);
            return Task.CompletedTask;
        }


        public Task ReceiveControlEvent<T>(T controlEvent) where T : ControlEvent
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method forcefully removes the callbacks. Only used for testing internal recompilation of callbacks (potentially dangerous!)
        /// </summary>
        protected void ForceDiscardCallbacks()
        {
            _callbacks.Clear();
        }
    }
}