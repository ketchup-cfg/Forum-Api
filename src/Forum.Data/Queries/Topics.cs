using Dapper;
using Forum.Data.Abstractions;
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
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as Description
                              from topics";

        return await connection.QueryAsync<Topic>(sql);
    }
    
    public async Task<Topic?> GetTopicById(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as Description
                               from topics
                              where id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Id = id});
    }

    public async Task<Topic?> GetTopicByName(string name)
    {
        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as description
                               from topics
                              where name = @Name";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Name = name});
    }


    public async Task<int> CreateTopic(Topic topic)
    {
        using var connection = _database.Connect();
        const string sql = @"insert into topics (
                                name,
                                description
                             )
                             values (
                                @Name,
                                @Description
                             )
                             returning Id;";
        
        return await connection.ExecuteScalarAsync<int>(sql,
            new
            {
                topic.Name,
                topic.Description,
            });
    }

    public async Task<int> UpdateTopic(Topic topic)
    {
        using var connection = _database.Connect();
        const string sql = @"update topics
                                set name = @Name
                                  , description = @Description
                              where id = @Id;";
        
        return await connection.ExecuteAsync(sql, 
            new 
            {
                Name = topic.Name,
                Description = topic.Description,
                Id = topic.Id
            });
    }

    public async Task<int> DeleteTopic(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"delete
                               from topics
                              where id = @Id;";

        return await connection.ExecuteAsync(sql, 
            new
            {
                Id = id
            });
    }
    
    public async Task<int> RemoveAll()
    {
        using var connection = _database.Connect();
        const string sql = @"delete from topics;";

        return await connection.ExecuteAsync(sql);
    }
}