﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using CueX.Geometry;

namespace CueX.Core.Subscription
{
    public class SubscriptionDetails
    {
        public EventFilter EventTypeFilter;
        public EventFilter OriginTypeFilter;
        public IArea Area;
    }
}