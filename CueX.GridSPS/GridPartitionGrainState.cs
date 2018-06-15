// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;

namespace CueX.GridSPS
{
    public class GridPartitionGrainState
    {
        public bool IsInitialized = false;
        public GridConfiguration Config = null;

        public List<ISpatialGrain> Children = new List<ISpatialGrain>();
        
        public readonly InterestManager InterestManager = new InterestManager();
        public readonly Dictionary<string /* EventType */, List<KeyValuePair<IGridPartitionGrain, double /* MaxDistance */>>> ForwardMap = new Dictionary<string, List<KeyValuePair<IGridPartitionGrain, double>>>();
    }
}