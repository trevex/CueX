// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CueX.Core.Subscription;

namespace CueX.Core
{
    public interface ISpatialGrainController
    {
        Task ReceiveControlEvent<T>(T logicEvent) where T : IControlEvent;
        Task<List<Guid>> GetStreamIdsForNewSubscription(SubscriptionDetails details);
    }
}