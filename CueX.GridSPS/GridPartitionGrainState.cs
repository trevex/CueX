// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CueX.Core;
using CueX.GridSPS.Config;

namespace CueX.GridSPS
{
    public class GridPartitionGrainState
    {
        public bool IsInitialized = false;
        public GridConfiguration Config = null;
        public readonly List<ISpatialGrain> Children = new List<ISpatialGrain>();
        public readonly InterestManager InterestManager = new InterestManager();
    }
}