// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using Orleans;
using Orleans.ApplicationParts;

namespace CueX.GridSPS.Config
{
    public static class HostHelper
    {
        public static void AddApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(GridManagementGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(GridPartitionGrain).Assembly).WithReferences();
        }
    }
}