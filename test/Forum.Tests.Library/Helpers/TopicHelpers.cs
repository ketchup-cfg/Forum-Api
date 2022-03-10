using Forum.Data.Abstractions;
using Forum.Data.Models;

namespace Forum.Tests.Library.Helpers;

public class TopicHelpers
{
    private readonly ITopics _topics;

    public TopicHelpers(ITopics topics)
    {
        _topics = topics;
    }
    
    /// <summary>
    /// Create a mock topic in the database for testing purposes, with a randomly generated string being used for the
    /// topic name.
    /// </summary>
    /// <returns>A mock topic with a randomly generated name.</returns>
    public async Task<Topic> CreateMockTopic(string description = "Test")
    {
        var topic = new Topic
        {
            Name = Guid.NewGuid().ToString(),
            Description = description
        };
        topic.Id = await _topics.CreateTopic(topic);

        return topic;
    }
}