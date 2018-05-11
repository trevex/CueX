// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Subscription
{
    public static class EventHelper
    {
        // TODO: in the long run it would probably be nice to use type UUIDs instead (requires research whether safe)
        public static string GetEventName<T>() where T : SpatialEvent
        {
            return GetTypeName<T>();
        }

        public static string GetTypeName<T>()
        {
            return typeof(T).ToString();
        }
    }
}