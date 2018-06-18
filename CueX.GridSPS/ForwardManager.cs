// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CueX.Core.Subscription;

namespace CueX.GridSPS
{
    public class ForwardManager
    {
        public Tuple<int, int> PartitionIndices;
        
        Task SetupForwardingIfNecessary(SubscriptionDetails details)
        {
            
            return Task.CompletedTask;
        }
    }
}