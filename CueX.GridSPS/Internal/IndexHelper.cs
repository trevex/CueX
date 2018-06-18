// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using CueX.Geometry;

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

        internal static Tuple<int, int> GetPartitionIndices(string key, double partitionSize)
        {
            var parts = key.Split(",");
            return new Tuple<int, int>(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
        }
    }
}