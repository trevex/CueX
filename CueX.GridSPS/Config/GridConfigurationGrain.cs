// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core.Controller;
using CueX.GridSPS.Controller;
using Orleans;

namespace CueX.GridSPS.Config
{
    public class GridConfigurationGrain : Grain, IGridConfigurationGrain
    {
        private readonly IGridConfigurationService _configService;
        private readonly IControllerService _controllerService;

        public static readonly string  DefaultKey = "default";
        
        public GridConfigurationGrain(IGridConfigurationService configService, IControllerService controllerService)
        {
            _configService = configService;
            _controllerService = controllerService;
        }
        
        public Task<GridConfiguration> GetConfiguration()
        {
            return _configService.GetConfiguration();
        }

        public async Task SetConfiguration(GridConfiguration config)
        {
            await _configService.SetConfiguration(config);
        }

        public Task SetController()
        {
            return _controllerService.SetController(new GridController());
        }

        public Task<IController> GetController()
        {
            return _controllerService.GetController();
        }
    }
}