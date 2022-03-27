using Dapper;
using Somewhere.Data.Abstractions;
using Somewhere.Data.Models;
using Npgsql;
using Somewhere.Core.Abstractions;
using Somewhere.Core.Exceptions;

namespace Somewhere.Core.Services;

public class TopicsService : ITopicsService
{
    private readonly IDatabase _database;

    public TopicsService(IDatabase database)
    {
        _database = database;
    }

    public async Task<bool> TopicExists(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from topics
                              where id = @Id";

        var numberOfTopicsWithId = await connection.ExecuteScalarAsync<int>(sql, new {Id = id});

        return numberOfTopicsWithId > 0;
    }

    public async Task<bool> TopicExists(string name)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from topics
                              where name = @Name";

        var numberOfTopicsWithName = await connection.ExecuteScalarAsync<int>(sql, new {Name = name});

        return numberOfTopicsWithName > 0;
    }

    public async Task<bool> NameIsUnique(string name)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from topics
                              where name = @Name";

        var numberOfTopicsWithName = await connection.ExecuteScalarAsync<int>(sql, new {Name = name});

        return numberOfTopicsWithName == 0;
    }

    public async Task<bool> NewNameIsUnique(int id, string newName)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from topics
                              where name = @NewName
                                and id <> @Id";

        var numberOfTopicsWithName = await connection.ExecuteScalarAsync<int>(sql, new {NewName = newName, Id = id});

        return numberOfTopicsWithName == 0;
    }

    public async Task<bool> NewIdIsUnique(int id, int newId)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from topics
                              where id = @NewId
                                and id <> @Id";

        var numberOfOtherTopicsWithId = await connection.ExecuteScalarAsync<int>(sql, new {NewId = newId, Id = id});

        return numberOfOtherTopicsWithId == 0;
    }

    public async Task<IEnumerable<Topic>> GetAllTopics(int limit = 30, int page = 1)
    {
        /*
         * If limit is negative, set the limit to 0 since we can't return a negative
         * number of topics.
         *
         * If limit is greater than 30, just set the limit to 30 since we don't want
         * to return a large number of topics in an HTTP request.
         */
        limit = limit < 0 ? 0 : (limit > 30 ? 30 : limit);

        /*
         * If page is negative or equal to 0, then set to 1.
         *
         * If this is not done, hen we will try to skip a negative number to topics in
         * the query, which is not valid.
         */
        page = page <= 0 ? 1 : page;

        var numberOfTopicsToSkip = (page - 1) * limit;

        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as Description
                              from topics
                             order by name
                             limit @Limit offset @Offset";

        return await connection.QueryAsync<Topic>(sql,
            new
            {
                Limit = limit,
                Offset = numberOfTopicsToSkip
            });
    }

    public async Task<Topic?> GetTopic(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as Description
                               from topics
                              where id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Id = id});
    }

    public async Task<Topic?> GetTopic(string name)
    {
        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as description
                               from topics
                              where name = @Name";

        return await connection.QueryFirstOrDefaultAsync<Topic>(sql, new {Name = name});
    }

    public async Task<Topic> AddTopic(Topic topic)
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

        var newId = await connection.ExecuteScalarAsync<int>(sql, new {topic.Name, topic.Description});

        return await GetNewTopic(newId);
    }
    
    public async Task<int> UpdateTopic(int id, Topic topic)
    {
        var numberOfRecordsUpdated = 0;
        using var connection = _database.Connect();
        const string sql = @"update topics
                                set id = @NewId
                                  , name = @NewName
                                  , description = @NewDescription
                              where id = @Id;";
        try
        {
            numberOfRecordsUpdated = await connection.ExecuteAsync(sql,
                new
                {
                    Id = id,
                    NewName = topic.Name,
                    NewDescription = topic.Description,
                    NewId = topic.Id
                });
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.NotNullViolation)
        {
            throw new NullTopicNameException();
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation &&
                                          e.ConstraintName == "topics_pkey")
        {
            throw new DuplicateIdException();
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation &&
                                          e.ConstraintName == "topics_name_key")
        {
            throw new DuplicateTopicNameException();
        }

        return numberOfRecordsUpdated;
    }

    public async Task<int> RemoveTopic(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"delete
                               from topics
                              where id = @Id;";

        return await connection.ExecuteAsync(sql, new {Id = id});
    }

    private async Task<Topic> GetNewTopic(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id          as Id
                                  , name        as Name
                                  , description as Description
                               from topics
                              where id = @Id";

        return await connection.QuerySingleAsync<Topic>(sql, new {Id = id});
    }
}