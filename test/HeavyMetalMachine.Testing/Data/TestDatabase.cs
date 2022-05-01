using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using HeavyMetalMachine.Core.Abstractions;
using Npgsql;

namespace HeavyMetalMachine.Testing.Data;

public class TestDatabase : IDatabase
{
    private readonly string _connectionString;

    public TestDatabase(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("HeavyMetalMachine") ?? throw new InvalidOperationException();
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public void Initialize()
    {
        DropPostsTable();
        CreatePostsTable();
    }

    /// <summary>
    /// Drop the posts table from the application database.
    /// </summary>
    private void DropPostsTable()
    {
        using var connection = Connect();
        const string sql = @"drop table if exists posts;";

        connection.Execute(sql);
    }

    /// <summary>
    /// Create the posts table in the application database.
    /// </summary>
    private void CreatePostsTable()
    {
        using var connection = Connect();
        const string sql = @"create table posts (
                                id      serial primary key,
                                title   text   not null,
                                content text
                             );";

        connection.Execute(sql);
    }
}