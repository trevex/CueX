// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core;
using Orleans;

namespace CueX.GridSPS.Config
{
    public class GridConfigurationGrain : Grain, IGridConfigurationGrain
    {
        public static readonly string  DefaultKey = "default";
        
        private readonly IControlService _controlService;
        
        public GridConfigurationGrain(IControlService controlService)
        {
            _controlService = controlService;
        }
        
        public async Task<GridConfiguration> GetConfiguration()
        {
            return ((GridSpatialGrainController)await _controlService.GetSpatialGrainController()).GetConfig();
        }

        public Task SetConfiguration(GridConfiguration config)
        {
            return _controlService.SetSpatialGrainController(new GridSpatialGrainController(config));
        }
    }
}