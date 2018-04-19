// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using System.Threading.Tasks;

namespace CueX.Core.Subscription
{
    public interface ISubscriptionSubject
    {
        Task<bool> Subscribe<T>(SubscriptionDetails subscription, Action<T> callback) where T : IEvent;
    }
}