﻿using CueX.Core;
using Orleans;

namespace SimpleExample.Grains
{
    public interface ISimpleGrain : ISpatialGrain, IGrainWithIntegerKey
    {
    }
}