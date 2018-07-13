// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CueX.Core;
using CueX.Core.Subscription;
using CueX.GridSPS.Config;
using CueX.GridSPS.Internal;

namespace CueX.GridSPS
{
    public class InterestManager
    {
        private double _partitionSize;
        private Tuple<int, int> _partitionIndices;
        
        private readonly Dictionary<string /* EventType */, Dictionary<ISpatialGrain, SubscriptionFilter>> _filters = new Dictionary<string, Dictionary<ISpatialGrain, SubscriptionFilter>>();
        private readonly Dictionary<ISpatialGrain, Dictionary<string/* EventType */, SubscriptionFilter>> _interests = new Dictionary<ISpatialGrain, Dictionary<string, SubscriptionFilter>>();
        
        private readonly Dictionary<string/* PartitionId */, Dictionary<string/* EventType */, Dictionary<ISpatialGrain, bool>>> _forwards = new Dictionary<string, Dictionary<string, Dictionary<ISpatialGrain, bool>>>();

        public void Initialize(double partitionSize, Tuple<int, int> partitionIndices)
        {
            _partitionSize = partitionSize;
            _partitionIndices = partitionIndices;
        }
        
        public bool Add<T>(T subscriber, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
        {
            // Check if a filter map exists for this event
            var result = _filters.TryGetValue(eventName, out var eventInterestFilters);
            if (!result) // If not, create the dictionary
            { 
                eventInterestFilters = new Dictionary<ISpatialGrain, SubscriptionFilter>();
                _filters[eventName] = eventInterestFilters;
            }
            // If the filter map exists, check if the grain already has a subscription
            else if (eventInterestFilters.ContainsKey(subscriber))
            {
                return false;
            }
            // Create the subscription filter for this grain 
            eventInterestFilters[subscriber] = filter;
            
            // Check if the interest map exists for this grain
            result = _interests.TryGetValue(subscriber, out var grainInterests);
            if (!result) // If not, create it
            {
                grainInterests = new Dictionary<string, SubscriptionFilter>();
                _interests[subscriber] = grainInterests;
            } 
            // Add the event to the grain
            grainInterests[eventName] = filter;
            return true;
        }

        public bool TryGetEventFilters(string eventName, out Dictionary<ISpatialGrain, SubscriptionFilter> eventFilters)
        {
            return _filters.TryGetValue(eventName, out eventFilters);
        }

        public bool TryGetSubscriptions<T>(T subscriber, out Dictionary<string, SubscriptionFilter> grainInterests) where T : ISpatialGrain
        {
            return _interests.TryGetValue(subscriber, out grainInterests);
        }

        public int GetInterestCount()
        {
            var count = 0;
            foreach (var interestPair in _interests)
            {
                count += interestPair.Value.Count;
            }
            return count;
        }
        
        public Queue<ForwardCommand> GetForwardDelta<T>(T subscriber, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
        {
            var queue = new Queue<ForwardCommand>();
            var aabb = filter.Area.GetBoundingBox();
            var bottomLeft = IndexHelper.GetPartitionIndicesForPosition(aabb.BottomLeft, _partitionSize);
            var topRight = IndexHelper.GetPartitionIndicesForPosition(aabb.TopRight, _partitionSize);

            for (var x = bottomLeft.Item1; x <= topRight.Item1; x++)
            {
                for (var y = bottomLeft.Item2; y <= topRight.Item2; y++)
                {
                    if (x == _partitionIndices.Item1 && y == _partitionIndices.Item2) continue;
                    var id = IndexHelper.GetPartitionKeyForIndices(x, y);
                    if (PartitionUpdateNecessary(subscriber, id, eventName, filter))
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

        private bool PartitionUpdateNecessary<T>(T subscriber, string partitionId, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
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

        public Tuple<int, int> GetPartitionIndices()
        {
            return _partitionIndices;
        } 
    }
}