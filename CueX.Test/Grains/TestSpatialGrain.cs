// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.Test.Events;

namespace CueX.Test.Grains
{
 
    public class TestSpatialGrain : SpatialGrain<ITestSpatialGrain, TestSpatialGrainState>, ITestSpatialGrain
    {
        public Task<bool> HasParent()
        {
            return Task.FromResult(State.Parent != null);
        }

        public Task SubscribeToTestEvent()
        {
            SubscribeTo<TestEvent>().ForEach(e =>
            {
                State.LastTestEventValue = e.Value;
                WriteStateAsync().GetAwaiter().GetResult();
            });
            return Task.CompletedTask;
        }

        public Task<string> GetLastTestEventValue()
        {
            return Task.FromResult(State.LastTestEventValue);
        }
    }
}