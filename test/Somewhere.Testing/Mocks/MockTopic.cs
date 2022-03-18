using Somewhere.Data.Models;

namespace Somewhere.Testing.Mocks;

public static class MockTopic
{
    public const int MinTopicId = 0;
    public const int MaxTopicId = 99;
    public const int MaxTopicsPerPage = 30;
    
    public static Topic Create(int id = MinTopicId, string? name = null, string? description = null)
    {
        return new Topic
        {
            Id = id,
            Name = name ?? Guid.NewGuid().ToString(),
            Description = description
        };
    }
    
    public static IEnumerable<Topic> CreateMany(int numberOfTopics)
    {
        var topics = new List<Topic>();

        for (var i = MinTopicId; i < numberOfTopics; i++)
        {
            topics.Add(new Topic
            {
                Id = i,
                Name = i.ToString(),
                Description = "Test"
            });
        }

        return topics;
    }
}