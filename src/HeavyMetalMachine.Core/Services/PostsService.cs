using Dapper;
using HeavyMetalMachine.Core.Abstractions;
using HeavyMetalMachine.Core.Models;
using IDatabase = HeavyMetalMachine.Core.Abstractions.IDatabase;

namespace HeavyMetalMachine.Core.Services;

public class PostsService : IPostsService
{
    private readonly Abstractions.IDatabase _database;

    public PostsService(Abstractions.IDatabase database)
    {
        _database = database;
    }

    public async Task<bool> PostExists(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select count(*)
                               from posts
                              where id = @Id";

        var numberOfPostsWithId = await connection.ExecuteScalarAsync<int>(sql, new {Id = id});

        return numberOfPostsWithId > 0;
    }

    public async Task<IEnumerable<Post>> GetAllPosts(int limit = 30, int page = 1)
    {
        /*
         * If limit is negative, set the limit to 0 since we can't return a negative
         * number of posts.
         *
         * If limit is greater than 30, just set the limit to 30 since we don't want
         * to return a large number of topics in an HTTP request.
         */
        limit = limit < 0 ? 0 : (limit > 30 ? 30 : limit);

        /*
         * If page is negative or equal to 0, then set to 1.
         *
         * If this is not done, then we will try to skip a negative number of posts in
         * the query, which is not valid.
         */
        page = page <= 0 ? 1 : page;

        var numberOfPostsToSkip = (page - 1) * limit;

        using var connection = _database.Connect();
        const string sql = @"select id      as Id
                                  , title   as Title
                                  , content as Content
                              from posts
                             order by title
                             limit @Limit offset @Offset";

        return await connection.QueryAsync<Post>(sql,
            new
            {
                Limit = limit,
                Offset = numberOfPostsToSkip
            });
    }

    public async Task<Post?> GetPost(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id      as Id
                                  , title   as Title
                                  , content as Content
                               from posts
                              where id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Post>(sql, new {Id = id});
    }

    public async Task<Post> AddPost(Post post)
    {
        using var connection = _database.Connect();
        const string sql = @"insert into posts (
                                title
                              , content
                             )
                             values (
                                @Title
                              , @Content
                             )
                             returning Id;";

        var newId = await connection.ExecuteScalarAsync<int>(sql, new {post.Title, post.Content});

        return await GetNewPost(newId);
    }
    
    public async Task<int> UpdatePost(int id, Post post)
    {
        var numberOfRecordsUpdated = 0;
        using var connection = _database.Connect();
        const string sql = @"update posts
                                set title = @NewTitle
                                  , content = @NewContent
                              where id = @Id;";
        
        numberOfRecordsUpdated = await connection.ExecuteAsync(sql, 
            new 
            {
                Id = id,
                NewTitle = post.Title,
                NewContent = post.Content,
            });

        return numberOfRecordsUpdated;
    }

    public async Task<int> RemovePost(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"delete
                               from posts
                              where id = @Id;";

        return await connection.ExecuteAsync(sql, new {Id = id});
    }

    private async Task<Post> GetNewPost(int id)
    {
        using var connection = _database.Connect();
        const string sql = @"select id      as Id
                                  , title   as Title
                                  , content as Content
                               from posts
                              where id = @Id";

        return await connection.QuerySingleAsync<Post>(sql, new {Id = id});
    }
}