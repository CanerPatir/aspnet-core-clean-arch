using Xunit;

namespace TestBase
{
    public abstract class TestBaseWithInMemoryDb : TestBaseWithIoC, IClassFixture<InMemoryDatabaseFixture>
    {
        protected static InMemoryDatabaseFixture InMemoryDb { get; private set; }
        

        protected TestBaseWithInMemoryDb(InMemoryDatabaseFixture inMemoryDatabaseFixture)
        {
            InMemoryDb = inMemoryDatabaseFixture;
        }

        public override void Dispose()
        {
            base.Dispose();
            InMemoryDb.Dispose();
        }
    }
}