using System;

namespace Data.Tests.Fixtures;

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