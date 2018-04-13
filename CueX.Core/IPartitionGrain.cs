// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace CueX.Core
{
    public interface IPartitionGrain : IGrain
    {
        Task Add<T>(T spatialGrain) where T : ISpatialGrain;
        Task<bool> Remove<T>(T spatialGrain) where T : ISpatialGrain;
        Task<IEnumerable<ISpatialGrain>> GetChildren();
    }
}