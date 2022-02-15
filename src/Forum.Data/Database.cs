using System.Data;
using Forum.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Forum.Data;

public class Database : IDatabase
{
    private readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }
}