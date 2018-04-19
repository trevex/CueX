using System;
using System.Threading.Tasks;
using CueX.Core;
using CueX.Core.Subscription;
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

        public async Task SubscribeToSimpleEvent()
        {
            await Subscribe<SimpleEvent>(new SubscriptionDetails
            {
                EventTypeName = EventHelper.GetEventName<SimpleEvent>()
            }, OnSimpleEvent);
        }

        public async Task OnSimpleEvent(SimpleEvent e)
        {
            Console.WriteLine(e.Value);
        }
        

    }
}