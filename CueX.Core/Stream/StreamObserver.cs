// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CueX.Core.Subscription;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace CueX.Core.Stream
{
    public class StreamObserver<T> : IAsyncObserver<T> where T : SpatialEvent
    {
        private readonly ILogger _logger;
        private Func<T, Task> _callback;
        
        public StreamObserver(ILogger logger, Func<T, Task> callback)
        {
            _logger = logger;
            _callback = callback;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("StreamObserver received stream completed event");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(System.Exception ex)
        {
            _logger.LogInformation($"StreamObserver is experiencing message delivery failure, ex :{ex}");
            return Task.CompletedTask;
        }

        public Task OnNextAsync(T item, StreamSequenceToken token = null)
        {
            _callback(item);
            return Task.CompletedTask;
        }
    }
}