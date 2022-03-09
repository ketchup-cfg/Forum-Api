using System;
using System.Data;
using Forum.Data.Interfaces;
using Npgsql;

namespace Data.Tests.Fixtures;

public class TestDatabase : IDatabase
{
    private readonly string _connectionString;
    
    public TestDatabase()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB");

        _connectionString =
            $"Server={host};Port={port};Database={database};User Id={user};Password={password};";
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }
}