using System;
using System.Threading.Tasks;
using CueX.Core;
using Microsoft.Extensions.Logging;

namespace SimpleExample.Grains
{
    public class SimpleGrainState : SpatialGrainState
    {
    }

    public class SimpleGrain : SpatialGrain<ISimpleGrain, SimpleGrainState>, ISimpleGrain
    {
        private readonly ILogger _logger;
        
        public SimpleGrain(ILogger<SimpleGrain> logger)
        {
            _logger = logger;
        }

        public Task SubscribeToSimpleEvent()
        {
            SubscribeTo<SimpleEvent>().ForEach(e => Console.WriteLine(e.Value));
            return Task.CompletedTask;
        }

    }
}