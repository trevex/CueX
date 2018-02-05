// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using Orleans;
using Orleans.ApplicationParts;

namespace CueX.GridSPS
{
    public static class GridConfigurationHelper
    {
        public static void AddGridHostApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(GridPartitionGrain).Assembly).WithReferences();
        }

        public static void AddGridClientApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(IGridPartitionGrain).Assembly);
        }
    }
}