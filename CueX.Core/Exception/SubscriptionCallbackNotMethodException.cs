﻿// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Exception
{

    public class SubscriptionCallbackNotMethodException: System.Exception
    {
        private const string DefaultMessage = "Subscription callbacks need to be class methods! "; 
        
        public SubscriptionCallbackNotMethodException()
            : base(DefaultMessage)
        {
        }

        public SubscriptionCallbackNotMethodException(string message)
            : base(message)
        {
        }

        public SubscriptionCallbackNotMethodException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}