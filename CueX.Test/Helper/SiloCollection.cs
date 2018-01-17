using Xunit;

namespace CueX.Test.Helper
{
    [CollectionDefinition(SiloCollection.Name)]
    public class SiloCollection : ICollectionFixture<SiloFixture>
    {
        public const string Name = "SiloCollection";
    }
}