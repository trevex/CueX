// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace CueX.Core
{
    public interface IControlService
    {
        Task<ISpatialGrainController> GetSpatialGrainController();
        Task SetSpatialGrainController(ISpatialGrainController config);
    }
}