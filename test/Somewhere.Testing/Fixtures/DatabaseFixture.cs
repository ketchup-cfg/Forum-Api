using Microsoft.Extensions.Configuration;
using Somewhere.Testing.Data;

namespace Somewhere.Testing.Fixtures;

public class DatabaseFixture : IDisposable
{
    public TestDatabase Database { get; }

    public DatabaseFixture()
    {
        var configFileName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing"
            ? "appsettings.Testing.json"
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