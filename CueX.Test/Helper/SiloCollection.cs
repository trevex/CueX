// Copyright (c) Niklas Voss. All rights reserved.
// Licensed under the Apache2 license. See LICENSE file in the project root for full license information.
using Xunit;

namespace CueX.Test.Helper
{
    [CollectionDefinition(SiloCollection.Name)]
    public class SiloCollection : ICollectionFixture<SiloFixture>
    {
        public const string Name = "SiloCollection";
    }
}