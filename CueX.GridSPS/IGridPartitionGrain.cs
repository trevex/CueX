// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
using Orleans;

namespace CueX.GridSPS
{
    public interface IGridPartitionGrain : IGrainWithStringKey
    {
        Task Add<T>(T spatialGrain) where T : ISpatialGrain;
        Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain;
        Task<IEnumerable<ISpatialGrain>> GetChildren();
        Task<bool> HandleSubscription<T>(T subscribingGrain, SubscriptionDetails details) where T : ISpatialGrain;
        Task HandleSpatialEvent<T>(T spatialEvent) where T : SpatialEvent;
        Task<int> GetInterestCount();
        Task<Tuple<int, int>> GetPartitionIndices();
    }
}