﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Controller;
using CueX.GridSPS.Controller;
using CueX.Test.Events;
using Microsoft.Extensions.Logging;

namespace CueX.Test.Grains
{
 
    public class TestSpatialGrain : SpatialGrain<ITestSpatialGrain, TestSpatialGrainState>, ITestSpatialGrain
    {
        
        public Task<bool> HasGridPartition()
        {
            return (State.Controller as GridController).HasPartition();
        }

        public async Task SubscribeToTestEvent()
        {
            await SubscribeTo<TestEvent>()
                .ForEach(OnTestEvent);
        }

        public Task OnTestEvent(TestEvent e)
        {
            State.LastTestEventValue = e.Value;
            return WriteStateAsync();
        }
        
        public Task<string> GetLastTestEventValue()
        {
            return Task.FromResult(State.LastTestEventValue);
        }

        public Task ForcefullyRecompileCallback()
        {
            ForceDiscardCallbacks();
            RecompileCallbacksIfNecessary();
            return Task.CompletedTask;
        }

    }
}