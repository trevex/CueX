// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using CueX.Core.Subscription;
using Orleans.Streams;

namespace CueX.Core.Stream
{
    public class AnyHandle<T> : IAnyHandle where T : SpatialEvent
    {
        private readonly StreamSubscriptionHandle<T> _internalHandle;

        public AnyHandle(StreamSubscriptionHandle<T> internalHandle)
        {
            _internalHandle = internalHandle;
        }

        public Task UnsubscribeAsync()
        {
            return _internalHandle.UnsubscribeAsync();
        }
    }
}