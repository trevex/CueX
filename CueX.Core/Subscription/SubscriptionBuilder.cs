// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using CueX.Core.Exception;

namespace CueX.Core.Subscription
{
    internal class SubscriptionBuilder<T> : ISubscriptionBuilder<T> where T : IEvent
    {
        private readonly SubscriptionDetails _details;
        private readonly ISubscriptionSubject _subject;
        
        public SubscriptionBuilder(ISubscriptionSubject subject, ITypeFilter eventFilter)
        {
            _subject = subject;
            _details = new SubscriptionDetails
            {
                EventFilter = eventFilter
            };
        }

        public void ForEach(Action<T> callback)
        {
            var result = _subject.Subscribe(_details, callback).GetAwaiter().GetResult();
            if (!result) throw new AlreadySubscribedException();
        }
    }
}