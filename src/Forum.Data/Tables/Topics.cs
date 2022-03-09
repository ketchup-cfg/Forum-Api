using Dapper;
using Forum.Data.Interfaces;
using Forum.Data.Models;

namespace Forum.Data.Tables;

public class Topics : ITopics, ITable
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
        const string sql = @"insert into topics
                            (
                                name,
                                description
                            )
                            values
                            (
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

    public void Initialize()
    {
        DropTable();
        CreateTable();
    }

    public async void DropTable()
    {
        using var connection = _database.Connect();
        const string sql = @"drop table if exists topics;";

        await connection.ExecuteAsync(sql);
    }

    public async void CreateTable()
    {
        using var connection = _database.Connect();
        const string sql = @"create table topics (
                                id          serial primary key,
                                name        text   not null unique,
                                description text
                             );";

        await connection.ExecuteAsync(sql);
    }

    public async void ClearTable()
    {
        using var connection = _database.Connect();
        const string sql = @"truncate topics;";

        await connection.ExecuteAsync(sql);
    }
}