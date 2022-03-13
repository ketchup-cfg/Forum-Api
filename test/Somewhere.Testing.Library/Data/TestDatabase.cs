using System.Data;
using Dapper;
using Somewhere.Data.Abstractions;
using Npgsql;

namespace Somewhere.Testing.Library.Data;

public class TestDatabase : IDatabase
{
    private readonly string _connectionString;
    
    public TestDatabase()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ??
                   throw new InvalidOperationException("POSTGRES_HOST environment variable not set");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ??
                   throw new InvalidOperationException("POSTGRES_PORT environment variable not set");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ??
                   throw new InvalidOperationException("POSTGRES_USER environment variable not set");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ??
                       throw new InvalidOperationException("POSTGRES_PASSWORD environment variable not set");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ??
                       throw new InvalidOperationException("POSTGRES_DB environment variable not set");

        _connectionString =
            $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public void Initialize()
    {
        DropTopicsTable();
        CreateTopicsTable();
    }
    
    /// <summary>
    /// Drop the topics table from the application database.
    /// </summary>
    private void DropTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"drop table if exists topics;";

        connection.Execute(sql);
    }

    /// <summary>
    /// Create the topics table in the application database.
    /// </summary>
    private void CreateTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"create table topics (
                                id          serial primary key,
                                name        text   not null unique,
                                description text
                             );";

        connection.Execute(sql);
    }
}