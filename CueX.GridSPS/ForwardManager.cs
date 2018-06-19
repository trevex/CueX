// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Internal;

namespace CueX.GridSPS
{
    public class ForwardManager
    {
        public double PartitionSize;
        public Tuple<int, int> PartitionIndices;
        
        private readonly Dictionary<string/* PartitionId */, Dictionary<string/* EventName */, Dictionary<ISpatialGrain, bool>>> _forwards = new Dictionary<string, Dictionary<string, Dictionary<ISpatialGrain, bool>>>();
        
        public Queue<ForwardCommand> GetForwardDelta<T>(T subscriber, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
        {
            var queue = new Queue<ForwardCommand>();
            var aabb = filter.Area.GetBoundingBox();
            var bottomLeft = IndexHelper.GetPartitionIndicesForPosition(aabb.BottomLeft, PartitionSize);
            var topRight = IndexHelper.GetPartitionIndicesForPosition(aabb.TopRight, PartitionSize);

            for (var x = bottomLeft.Item1; x <= topRight.Item1; x++)
            {
                for (var y = bottomLeft.Item2; y <= topRight.Item2; y++)
                {
                    if (x == PartitionIndices.Item1 && y == PartitionIndices.Item2) continue;
                    var id = IndexHelper.GetPartitionKeyForIndices(x, y);
                    if (PartitionUpdate(subscriber, id, eventName, filter))
                    {
                        queue.Enqueue(new ForwardCommand
                        {
                            PartitionId = id,
                            EventName = eventName,
                            ForwardState = ForwardState.StartForwarding
                        });
                    }
                }
            }
            return queue;
        }

        private bool PartitionUpdate<T>(T subscriber, string partitionId, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
        {
            var result = _forwards.TryGetValue(partitionId, out var partitionForwards);
            if (!result)
            {
                partitionForwards = new Dictionary<string, Dictionary<ISpatialGrain, bool>>();
                _forwards[partitionId] = partitionForwards;
            }

            result = partitionForwards.TryGetValue(eventName, out var subscriberForwards);
            if (!result)
            {
                subscriberForwards = new Dictionary<ISpatialGrain, bool>();
                partitionForwards[eventName] = subscriberForwards;
            }

            if (subscriberForwards.ContainsKey(subscriber))
            {
                return false;
            }

            subscriberForwards[subscriber] = true;
            return true;
        }
    }
}