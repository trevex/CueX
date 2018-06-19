using System;
using CueX.Core.Subscription;
using CueX.Geometry;
using CueX.GridSPS;
using CueX.Test.Grains;
using Xunit;

namespace CueX.Test
{
    public class GridForwardManagerTest
    {
        [Fact]
        public async void TestForwardCommandQueueDelta()
        {
            var fm = new ForwardManager
            {
                PartitionIndices = new Tuple<int, int>(2, 2),
                PartitionSize = 2d
            };
            var filter = new SubscriptionFilter
            {
                Area = Circle.WithRadius(2d)
            };
            filter.Area.SetOrigin(new Vector3d(5d, 5d, 0d));
            var subscriber = new SpatialGrainStub();
            var queue1 = fm.GetForwardDelta(subscriber, "TEST", filter);
            Assert.Equal(queue1.Count, 8);
            // All forwards for event TEST exist so same subscription should result in no commands
            var queue2 = fm.GetForwardDelta(subscriber, "TEST", filter);
            Assert.Equal(queue2.Count, 0);
        }
    }
}