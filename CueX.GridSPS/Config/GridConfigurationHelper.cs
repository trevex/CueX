// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using CueX.Core;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.ApplicationParts;

// TODO: refactor ISpatialGrain, IControlService and all other core functionality into Core namespace.

namespace CueX.GridSPS.Config
{
    public static class GridConfigurationHelper
    {
        public static void AddClientApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(ISpatialGrain).Assembly);
            parts.AddApplicationPart(typeof(IGridConfigurationGrain).Assembly);
        }
        
        public static void AddSiloApplicationParts(IApplicationPartManager parts)
        {
            parts.AddApplicationPart(typeof(GridConfigurationGrain).Assembly).WithReferences();
        }

        public static void AddServices(IServiceCollection svc)
        {
            svc.AddSingleton<IControlService, ControlService>();
        }
    }
}