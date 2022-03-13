using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Somewhere.Data.Abstractions;

namespace Somewhere.Data;

public class Database : IDatabase
{
    private readonly string _connectionString;

    public Database(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Somewhere");
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