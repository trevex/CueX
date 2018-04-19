// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

namespace CueX.Core.Exception
{

    public class AlreadySubscribedException: System.Exception
    {
        public AlreadySubscribedException()
        {
        }

        public AlreadySubscribedException(string message)
            : base(message)
        {
        }

        public AlreadySubscribedException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}