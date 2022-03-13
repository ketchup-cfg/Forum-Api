using Somewhere.Data.Models;
using Somewhere.Services.Abstractions;

namespace Somewhere.Testing.Library.Helpers;

public class TopicHelpers
{
    private readonly ITopicsService _topics;

    public TopicHelpers(ITopicsService topics)
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

        return await _topics.AddTopic(topic);
    }

    /// <summary>
    /// Create a set number of topics in the database to ensure that enough data exists for testing purposes.
    /// </summary>
    /// <param name="numberOfTopics">The number of topics to create.</param>
    public async Task EnsureSetNumberOfTopicsExist(int numberOfTopics)
    {
        for (var i = 0; i <= numberOfTopics; i++)
        {
            _ = await CreateMockTopic();
        }
    }
}