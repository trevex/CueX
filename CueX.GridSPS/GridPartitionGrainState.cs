// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using CueX.Core;
using CueX.GridSPS.Config;

namespace CueX.GridSPS
{
    public class GridPartitionGrainState : PartitionGrainState
    {
        public bool IsInitialized = false;
        public GridConfiguration Config = null;
    }
}