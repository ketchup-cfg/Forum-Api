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
        _connectionString = Environment.GetEnvironmentVariable("FORUM_TEST_DB") ??
                            throw new InvalidOperationException("FORUM_TEST_DB environment variable is not set");
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }
}