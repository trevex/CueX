// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using Orleans;

namespace CueX.GridSPS.Config
{
    /// <summary>
    /// Just a proxy to access the IGridConfigurationService from the client
    /// </summary>
    public interface IGridConfigurationGrain : IGrainWithStringKey
    {
        Task<GridConfiguration> GetConfiguration();
        Task SetConfiguration(GridConfiguration config);
    }
}