// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CueX.Core.Subscription;
using CueX.Numerics;
using Orleans;

namespace CueX.Core
{
    /// <summary>
    /// Abstract class providing the basic functionality necessary for sub-classes
    /// to be insertable into the PubSub-System.
    /// </summary>
    /// <typeparam name="TState">Application-specific state data type, that also holds <see cref="SpatialGrainState"/>.</typeparam>
    /// <typeparam name="TGrainInterface"></typeparam>
    public abstract class SpatialGrain<TGrainInterface, TState> : Grain<TState>, ISpatialGrain
        where TState : SpatialGrainState, new() where TGrainInterface : ISpatialGrain
    {
        private readonly Dictionary<string, Func<IEvent, Task>> _callbacks = new Dictionary<string, Func<IEvent, Task>>();

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
                    var interfaceParam = Expression.Parameter(typeof(IEvent));
                    var eventParam = Expression.Convert(interfaceParam, targetType);
                    var lambdaExpression = Expression.Lambda<Func<IEvent, Task>>(
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

        public async Task<bool> Subscribe<T>(SubscriptionDetails details, Func<T, Task> callback) where T : IEvent
        {
            var result = await State.Parent.HandleSubscription(this.AsReference<TGrainInterface>(), details);
            if (!result) return false;
            var method = callback.Method;
            if (method.DeclaringType == null || !CodeGenerator.IsValidLanguageIndependentIdentifier(method.Name))
            {   // TODO: throw custom exception!
                throw new System.Exception("Needs to be method!");
            }
            _callbacks[EventHelper.GetEventName<T>()] = e => callback((T) e); // TODO: improve this hack, once basic SPS is working
            State.CallbackMethodInfos[typeof(T)] = callback.Method; // save reflection info to reconstruct callbacks during activation
            await WriteStateAsync();
            return true;
        }

        public Task ReceiveEvent(string eventTypeName, IEvent eventValue)
        {
            _callbacks[eventTypeName](eventValue);
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