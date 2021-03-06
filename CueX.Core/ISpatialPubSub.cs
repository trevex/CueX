﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core.Subscription;

namespace CueX.Core
{
    public interface ISpatialPubSub
    {
        Task Insert<T>(T spatialGrain) where T : ISpatialGrain;
        Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain;
        Task Dispatch<T>(T spatialEvent) where T : SpatialEvent;
    }
}