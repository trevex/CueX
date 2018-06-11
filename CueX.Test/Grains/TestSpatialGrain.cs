// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core;
using CueX.Test.Events;
using Microsoft.Extensions.Logging;

namespace CueX.Test.Grains
{
 
    public class TestSpatialGrain : SpatialGrain<ITestSpatialGrain, TestSpatialGrainState>, ITestSpatialGrain
    {
        public TestSpatialGrain(ILogger<SpatialGrain<ITestSpatialGrain, TestSpatialGrainState>> logger, IControlService controlService) : base(logger, controlService)
        {
        }

        public async Task SubscribeToTestEvent()
        {
            await SubscribeTo<TestEvent>()
                .ForEach(OnTestEvent);
        }

        public async Task OnTestEvent(TestEvent e)
        {
            State.LastTestEventValue = e.Value;
            await WriteStateAsync();
        }
        
        public Task<string> GetLastTestEventValue()
        {
            return Task.FromResult(State.LastTestEventValue);
        }

        public Task ForcefullyRecompileCallback()
        {
//            ForceDiscardCallbacks();
//            RecompileCallbacksIfNecessary();
            return Task.CompletedTask;
        }

    }
}