// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.ApplicationParts;

namespace CueX.GridSPS.Config
{
    public static class GridConfigurationHelper
    {
        public static void AddClientApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(IGridManagementGrain).Assembly);
            parts.AddApplicationPart(typeof(IGridPartitionGrain).Assembly);
            parts.AddApplicationPart(typeof(IGridConfigurationGrain).Assembly);
        }
        
        public static void AddSiloApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(GridManagementGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(GridPartitionGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(GridConfigurationGrain).Assembly).WithReferences();
        }

        public static void AddServices(IServiceCollection svc)
        {
            svc.AddSingleton<IGridConfigurationService, GridConfigurationService>();
        }
    }
}