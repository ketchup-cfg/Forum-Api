using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using IDatabase = HeavyMetalMachine.Core.Abstractions.IDatabase;

namespace HeavyMetalMachine.Core.Services;

public class Database : IDatabase
{
    private readonly string _connectionString;

    public Database(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("HeavyMetalMachine");
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }

    /// <summary>
    /// Initialize the tables for the application database, ensuring to also drop the tables if they already exist.  
    /// </summary>
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