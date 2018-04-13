// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.\

using System;

namespace CueX.Core.Subscription
{
    public interface ISubscriptionBuilder<T> where T : IEvent
    {
        void ForEach(Action<T> callback);
    }
}