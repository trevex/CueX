// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using System;

namespace CueX.Test.Helper
{
    public class ClusterFixture : IDisposable
    {
        public ClusterFixture()
        {
            Cluster = new TestCluster();
        }

        public void Dispose()
        {
        }

        public TestCluster Cluster { get; private set; }
    }
}