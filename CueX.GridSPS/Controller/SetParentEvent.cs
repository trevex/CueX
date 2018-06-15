// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using CueX.Core.Controller;

namespace CueX.GridSPS.Controller
{
    public class SetParentEvent : ControlEvent
    {
        public IGridPartitionGrain Partition;
    }
}