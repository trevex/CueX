// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using Orleans;

namespace CueX.GridSPS.Config
{
    public class GridConfigurationGrain : Grain, IGridConfigurationGrain
    {
        private readonly IGridConfigurationService _configService;

        public static readonly string  DefaultKey = "default";
        
        public GridConfigurationGrain(IGridConfigurationService configService)
        {
            this._configService = configService;
        }
        
        public Task<GridConfiguration> GetConfiguration()
        {
            return _configService.GetConfiguration();
        }

        public Task SetConfiguration(GridConfiguration config)
        {
            return _configService.SetConfiguration(config);
        }
    }
}