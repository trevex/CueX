// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;

namespace CueX.GridSPS
{
    public class GridPartitionGrainState : PartitionGrainState
    {
        public bool IsInitialized = false;
        public GridConfiguration Config = null;

        public readonly Dictionary<string /* EventType */, Dictionary<ISpatialGrain, SubscriptionFilter>> InterestFilterMap = new Dictionary<string, Dictionary<ISpatialGrain, SubscriptionFilter>>();
        public readonly Dictionary<ISpatialGrain, List<string/* EventType */>> GrainInterestMap = new Dictionary<ISpatialGrain, List<string>>();
        public readonly Dictionary<string/* EventType */, List<IGridPartitionGrain>> ForwardMap = new Dictionary<string, List<IGridPartitionGrain>>();
    }
}