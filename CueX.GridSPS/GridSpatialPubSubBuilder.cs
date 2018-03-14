// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using CueX.API;
using CueX.Core;
using CueX.GridSPS.Config;
using Orleans;

namespace CueX.GridSPS
{
    public class GridSpatialPubSubBuilder : SpatialPubSubBuilder
    {
        private GridConfiguration _config =  GridConfiguration.Default();

        public GridSpatialPubSubBuilder Configure(Action<GridConfiguration> configure)
        {
            configure(_config);
            return this;
        }
        
        public override ISpatialPubSub Build(IClusterClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}