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
        public SimpleGrain(ILogger<SpatialGrain<ISimpleGrain, SimpleGrainState>> logger, IControlService controlService) : base(logger, controlService)
        {
        }

        public async Task SubscribeToSimpleEvent()
        {
            await SubscribeTo<SimpleEvent>()
                .ForEach(OnSimpleEvent);
        }

        public async Task OnSimpleEvent(SimpleEvent e)
        {
            Console.WriteLine(e.Value);
        }
    }
}