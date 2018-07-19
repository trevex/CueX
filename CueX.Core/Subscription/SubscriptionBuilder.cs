// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CueX.Geometry;

namespace CueX.Core.Subscription
{
    public class SubscriptionBuilder<T> where T : SpatialEvent
    {
        private readonly ISubscriptionSubject _subject;
        private SubscriptionDetails _details;
        
        public SubscriptionBuilder(ISubscriptionSubject subject)
        {
            _subject = subject;
            _details = new SubscriptionDetails
            {
                EventTypeFilter = EventFilter.ForType<T>(),
                Area = Circle.WithRadius(1d) // Default, if user does not specify area
            };
        }

        public SubscriptionBuilder<T> InRadius(double radius)
        {
            _details.Area = Circle.WithRadius(radius);
            return this;
        }

        public async Task<bool> ForEach(Func<T, Task> callback)
        {
            return await _subject.SubscribeWithDetails(_details, callback);
        }
    }
}