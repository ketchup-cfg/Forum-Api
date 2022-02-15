using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Forum.Data;
using Microsoft.Extensions.Configuration;

namespace Data.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public Database Database { get; }
    
    public DatabaseFixture()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();
        Database = new Database(config);
    }
    
    public void Dispose()
    {
        
    }
}