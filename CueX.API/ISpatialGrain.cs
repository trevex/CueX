// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Numerics;
using Orleans;


namespace CueX.API
{
    public interface ISpatialGrain<T> : IGrainWithIntegerKey where T : struct
    {
        Vector<T> GetPosition();
    }

}