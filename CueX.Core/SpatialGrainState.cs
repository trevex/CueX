// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Numerics;

namespace CueX.Core
{
    public class SpatialGrainState<TPrecision> where TPrecision : struct
    {
        public Vector<TPrecision> Position;
    }
}