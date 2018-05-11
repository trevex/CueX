// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using CueX.Geometry;

namespace CueX.Core
{
    public abstract class SpatialGrainState
    {
        public Vector3d Position;
        public IPartitionGrain Parent;
        public Dictionary<Type /* EventType */, MethodInfo> CallbackMethodInfos = new Dictionary<Type, MethodInfo>();
    }
}