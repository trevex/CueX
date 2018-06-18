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
        public Tuple<int, int> PartitionIndices;
        
        private Dictionary<string/* PartitionId */, Dictionary<string/* EventName */, Dictionary<ISpatialGrain, bool>>> _forwards = new Dictionary<string, Dictionary<string, Dictionary<ISpatialGrain, bool>>>();
        
        public Queue<ForwardCommand> GetForwardDelta<T>(T subscriber, string eventName, SubscriptionFilter filter) where T : ISpatialGrain
        {
            var queue = new Queue<ForwardCommand>();
            var done = false;
            var gridRange = 0;
            while (!done)
            {    
                
                // TODO: REWRITE! USE AABB TO GET GRID INTERSECTS LEL
                // TODO: MAYBE MOVE FORWARDING INTO INTEREST MANAGEMENT :X
                // Increase gridRange and set done to true, it is falsed if any update 
                gridRange += 1;
                done = true;
                // Check top and bottom row of rectangle edges
                for (var x = -gridRange; x <= gridRange; x++)
                {
                    var top = IndexHelper.GetPartitionKeyForIndices(x, gridRange);
                    if (PartitionUpdate(subscriber, top, eventName, filter))
                    {
                        done = false;
                        queue.Enqueue(new ForwardCommand
                        {
                            PartitionId = top,
                            EventName = eventName,
                            ForwardState = ForwardState.StartForwarding
                        });
                    }
                    var bottom = IndexHelper.GetPartitionKeyForIndices(x, -gridRange);
                    if (PartitionUpdate(subscriber, bottom, eventName, filter))
                    {
                        done = false;
                        queue.Enqueue(new ForwardCommand
                        {
                            PartitionId = bottom,
                            EventName = eventName,
                            ForwardState = ForwardState.StartForwarding
                        });
                    }
                }
                // Last check left and right inbetween edge
                for (var y = -gridRange + 1; y <= gridRange - 1; y++)
                {
                    var left = IndexHelper.GetPartitionKeyForIndices(-gridRange, y);
                    if (PartitionUpdate(subscriber, left, eventName, filter))
                    {
                        done = false;
                        queue.Enqueue(new ForwardCommand
                        {
                            PartitionId = left,
                            EventName = eventName,
                            ForwardState = ForwardState.StartForwarding
                        });
                    }
                    var right = IndexHelper.GetPartitionKeyForIndices(gridRange, y);
                    if (PartitionUpdate(subscriber, right, eventName, filter))
                    {
                        done = false;
                        queue.Enqueue(new ForwardCommand
                        {
                            PartitionId = right,
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