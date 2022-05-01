using HeavyMetalMachine.Core.Models;

namespace HeavyMetalMachine.Testing.Mocks;

public static class MockPost
{
    public const int MinPostId = 0;
    public const int MaxPostId = 99;
    public const int MaxPostsPerPage = 30;

    public static Post Create(int id = MinPostId, string? title = null, string? content = null)
    {
        return new Post
        {
            Id = id,
            Title = title ?? string.Empty,
            Content = content
        };
    }

    public static IEnumerable<Post> CreateMany(int numberOfPosts)
    {
        var posts = new List<Post>();

        for (var i = MinPostId; i < numberOfPosts; i++)
        {
            posts.Add(new Post
            {
                Id = i,
                Title = i.ToString(),
                Content = "Test"
            });
        }

        return posts;
    }
}