using CueX.Core.Subscription;

namespace SimpleExample.Grains
{
    public class SimpleEvent : IEvent
    {
        public string Value;
    }
}