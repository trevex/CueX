// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Subscription
{
    public static class EventHelper
    {
        public static string GetEventName<T>() where T : SpatialEvent
        {
            return typeof(T).ToString();
        }
    }
}