// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace CueX.Core
{
    public class ControlService : IControlService
    {
            private ISpatialGrainController _spatialGrainController = null;
        
            public Task<ISpatialGrainController> GetSpatialGrainController()
            {
                return Task.FromResult(_spatialGrainController);
            }

            public Task SetSpatialGrainController(ISpatialGrainController spatialGrainController)
            {
                _spatialGrainController = spatialGrainController;
                return Task.CompletedTask;
            }
        }
    }
}