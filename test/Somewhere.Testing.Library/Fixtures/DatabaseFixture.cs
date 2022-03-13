using Microsoft.Extensions.Configuration;
using Somewhere.Testing.Library.Data;

namespace Somewhere.Testing.Library.Fixtures;

public class DatabaseFixture : IDisposable
{
    public TestDatabase Database { get; }
    
    public DatabaseFixture()
    {
        var configFileName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "GITHUB"
            ? "appsettings.GitHub.json"
            : "appsettings.Development.json";

        var config = new ConfigurationBuilder()
            .AddJsonFile(configFileName)
            .Build();
        Database = new TestDatabase(config);
        Database.Initialize();
    }
    
    public void Dispose()
    {

    }
}