using Dapper;
using Forum.Data.Interfaces;
using Forum.Data.Models;

namespace Forum.Data.Queries;

public class Topics : ITopics
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
    
    public async Task<Topic?> GetTopicById(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id   as Id
                                  , name as Name
                               from topics
                              where id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Id = id});
    }
    
    public async Task<Topic?> GetTopicByName(string name)
    {
        using var connection = _database.Connect();
        const string sql = @"select id   as Id
                                  , name as Name
                               from topics
                              where name = @Name";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Name = name});
    }
}