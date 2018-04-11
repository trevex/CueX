// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;

namespace CueX.GridSPS.Config
{
    public class GridConfigurationService : IGridConfigurationService 
    {
        private GridConfiguration _config = null;
        
        public Task<GridConfiguration> GetConfiguration()
        {
            return Task.FromResult(_config);
        }

        public Task SetConfiguration(GridConfiguration config)
        {
            _config = config;
            return Task.CompletedTask;
        }
    }
}