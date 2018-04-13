// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using CueX.Numerics;

namespace CueX.GridSPS.Internal
{
    internal static class IndexHelper
    {
        internal static string GetPartitionKeyForPosition(Vector3d position, double partitionSize)
        {
            var x = position.X / partitionSize;
            var y = position.Y / partitionSize;
            return Math.Floor(x) + "," + Math.Floor(y);
        }        
    }
}