// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;

namespace CueX.GridSPS.Config
{
    public interface IGridConfigurationService
    {
        Task<GridConfiguration> GetConfiguration();
        Task SetConfiguration(GridConfiguration config);
    }
}