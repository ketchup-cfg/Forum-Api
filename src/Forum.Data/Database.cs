using System.Data;
using Dapper;
using Forum.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Forum.Data;

public class Database : IDatabase
{
    private readonly string _connectionString;

    public Database(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Forum");
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

    private async void DropTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"drop table if exists topics;";

        await connection.ExecuteAsync(sql);
    }
    
    private async void CreateTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"create table topics (
                                id          serial primary key,
                                name        text   not null unique,
                                description text
                            );";

        await connection.ExecuteAsync(sql);
    }
}