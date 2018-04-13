// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;

namespace CueX.Core.Subscription
{
    internal class SubscriptionBuilder<T> : ISubscriptionBuilder<T> where T : IEvent
    {
        private SubscriptionDetails _subscription;
        
        public SubscriptionBuilder(ISpatialGrain subscriber, ITypeFilter eventFilter)
        {
            _subscription = new SubscriptionDetails
            {
                Subscriber = subscriber,
                EventFilter = eventFilter
            };
        }

        public void ForEach(Action<T> callback)
        {
            // TODO: implementation
        }
    }
}