﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.API;
using CueX.Numerics;
using Orleans;

namespace CueX.Test.Grains
{
    public interface IBasicSpatialGrain : ISpatialGrain, IGrainWithIntegerKey
    {
        Task<bool> HasParent();
    }
}