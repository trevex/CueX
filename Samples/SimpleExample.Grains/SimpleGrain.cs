// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CueX.Core;
using CueX.Numerics;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace SimpleExample.Grains
{
    public class SimpleGrainState : SpatialGrainState
    {
    }

    public class SimpleGrain : SpatialGrain<ISimpleGrain, SimpleGrainState>, ISimpleGrain
    {
        private readonly ILogger _logger;
        
        public SimpleGrain(ILogger<SimpleGrain> logger)
        {
            _logger = logger;
        }

    }
}