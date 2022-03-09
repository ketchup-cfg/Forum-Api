using Forum.Tests.Library.Data;

namespace Forum.Tests.Library.Fixtures;

public class DatabaseFixture : IDisposable
{
    public TestDatabase Database { get; }
    
    public DatabaseFixture()
    {
        Database = new TestDatabase();
    }
    
    public void Dispose()
    {
        
    }
}