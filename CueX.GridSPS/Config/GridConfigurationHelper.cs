// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using CueX.Core;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.ApplicationParts;

namespace CueX.GridSPS.Config
{
    public static class GridConfigurationHelper
    {
        public static void AddClientApplicationParts(IApplicationPartManager parts)
        {
            // Add the base classes as well since they use IGrain
            parts.AddApplicationPart(typeof(IPartitionGrain).Assembly);
            parts.AddApplicationPart(typeof(ISpatialGrain).Assembly);
            // Add own grain interfaces
            parts.AddApplicationPart(typeof(IGridPartitionGrain).Assembly);
            parts.AddApplicationPart(typeof(IGridConfigurationGrain).Assembly);
        }
        
        public static void AddSiloApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(GridPartitionGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(GridConfigurationGrain).Assembly).WithReferences();
        }

        public static void AddServices(IServiceCollection svc)
        {
            svc.AddSingleton<IGridConfigurationService, GridConfigurationService>();
        }
    }
}