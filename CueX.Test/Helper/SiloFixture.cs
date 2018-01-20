// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;
using Orleans.TestKit;

namespace CueX.Test.Helper
{
    public class SiloFixture : IDisposable
    {
        public SiloFixture()
        {
            Silo = new TestKitSilo();
        }

        public void Dispose()
        {
        }

        public TestKitSilo Silo { get; private set; }
    }
}