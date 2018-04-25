// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Exception
{

    public class SubscriptionCallbackNotMemberException: System.Exception
    {
        private const string DefaultMessage = "Subscription callbacks need to be public members of the spatial grain they refer to! "; 
        
        public SubscriptionCallbackNotMemberException()
            : base(DefaultMessage)
        {
        }

        public SubscriptionCallbackNotMemberException(string message)
            : base(message)
        {
        }

        public SubscriptionCallbackNotMemberException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}