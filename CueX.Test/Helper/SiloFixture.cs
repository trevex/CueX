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