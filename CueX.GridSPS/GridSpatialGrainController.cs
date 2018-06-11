// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;

namespace CueX.GridSPS
{
    public class GridSpatialGrainController : ISpatialGrainController
    {
        private readonly GridConfiguration _config;
        
        public GridSpatialGrainController(GridConfiguration config)
        {
            _config = config;
        }

        public GridConfiguration GetConfig()
        {
            return _config;
        }

        public Task ReceiveControlEvent<T>(T logicEvent) where T : IControlEvent
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> GetStreamIdsForNewSubscription(SubscriptionDetails details)
        {
            var ids = new List<Guid>();
            
        }
    }
}