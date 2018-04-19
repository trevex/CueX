// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CueX.Core.Subscription;
using CueX.Numerics;

namespace CueX.Core
{
    public abstract class SpatialGrainState
    {
        public Vector3d Position;
        public IPartitionGrain Parent;
        public Dictionary<string /* EventType */, Action<IEvent>> Callbacks = new Dictionary<string, Action<IEvent>>();
    }
}