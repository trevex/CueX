﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Subscription
{
    public class SubscriptionFilter
    {
        public IArea Area;
        // TODO: add origin type
        
        public bool IsApplicable(SpatialEvent e)
        {
            // TODO: implement proper filtering
            return true;
        } 
    }
}