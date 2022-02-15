using Dapper;
using Forum.Data.Interfaces;
using Forum.Data.Models;

namespace Forum.Data.Queries;

public class Topics
{
    private readonly IDatabase _database;

    public Topics(IDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Topic>> GetAll()
    {
        using var connection = _database.Connect();
        const string sql = @"select id   as Id
                                  , name as Name
                              from topics";

        return await connection.QueryAsync<Topic>(sql);
    }
    
}