// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using CueX.API;
using Orleans;

namespace CueX.GridSPS
{
    public interface IGridPartitionGrain : IPartitionGrain, IGrainWithIntegerKey
    {
        
    }
}