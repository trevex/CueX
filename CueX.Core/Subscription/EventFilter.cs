// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Subscription
{
    public class EventFilter
    {
        private string _typename;

        private EventFilter()
        {
        }

        private EventFilter(string typename)
        {
            _typename = typename;
        }

        public static EventFilter ForType<T>()
        {
            return new EventFilter(EventHelper.GetTypeName<T>());
        }

        public bool SameAs<T>()
        {
            return EventHelper.GetTypeName<T>() == _typename;
        }

        public bool SameAs(string typename)
        {
            return typename == _typename;
        }

        public string GetTypename()
        {
            return _typename;
        }
    }
}