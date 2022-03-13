using Somewhere.Testing.Library.Data;

namespace Somewhere.Testing.Library.Fixtures;

public class DatabaseFixture : IDisposable
{
    public TestDatabase Database { get; }
    
    public DatabaseFixture()
    {
        Database = new TestDatabase();
        Database.Initialize();
    }
    
    public void Dispose()
    {

    }
}