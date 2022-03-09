using System;
using Forum.Data;

namespace Data.Tests.Fixtures;

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