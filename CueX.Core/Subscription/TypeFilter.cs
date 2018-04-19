// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
namespace CueX.Core.Subscription
{
    public class TypeFilter<TSelf> : ITypeFilter
    {
        public string GetTypeString()
        {
            return typeof(TSelf).ToString();
        }

        public bool IsType<TOther>()
        {
            return typeof(TSelf) == typeof(TOther);
        }
    }
}