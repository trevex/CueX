// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using CueX.Core;
using CueX.Core.Subscription;

namespace CueX.GridSPS
{
    public class InterestManager
    {
        private readonly Dictionary<string /* EventType */, Dictionary<ISpatialGrain, SubscriptionFilter>> _filters = new Dictionary<string, Dictionary<ISpatialGrain, SubscriptionFilter>>();
        private readonly Dictionary<ISpatialGrain, List<string/* EventType */>> _interests = new Dictionary<ISpatialGrain, List<string>>();
 
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
                grainInterests = new List<string>();
                _interests[subscriber] = grainInterests;
            } 
            // Add the event to the grain
            grainInterests.Add(eventName);
            return true;
        }

        public bool TryGetEventFilters(string eventName, out Dictionary<ISpatialGrain, SubscriptionFilter> eventFilters)
        {
            return _filters.TryGetValue(eventName, out eventFilters);
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

        public Dictionary<string, SubscriptionFilter> GetSubscriptions<T>(T subscriber) where T : ISpatialGrain
        {
            if (!_interests.TryGetValue(subscriber, out var grainInterests))
                return null;
            // If exis, collect all subscription filters
            var subs = new Dictionary<string, SubscriptionFilter>();
            foreach (var eventName in grainInterests)
            {
                subs[eventName] = _filters[eventName][subscriber];
            }
            return subs;
        }
    }
}