// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.

using System;
using System.Numerics;

namespace CueX.Core
{
    public static class SpatialPubSubConfigurationHelper
    {
        public static bool CheckHardwareSupport()
        {
            if (!Vector.IsHardwareAccelerated)
            {
                // TODO: throw warning
                Console.WriteLine("Hardware acceleration for Vectors is unavailable!");
                return false;
            }

            return true;
        }
    }
}