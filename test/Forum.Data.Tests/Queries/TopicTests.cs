using System;
using System.Threading.Tasks;
using Forum.Data.Models;
using Forum.Data.Queries;
using Forum.Tests.Library.Fixtures;
using Xunit;

namespace Forum.Data.Tests.Queries;

public class TopicTests : IClassFixture<DatabaseFixture>
{
    private readonly Topics _topics;

    public TopicTests(DatabaseFixture fixture)
    {
        _topics = new Topics(fixture.Database);
    }

    [Fact]
    public async void GetAll_NoTopics_ReturnsEmptyList()
    {
        // Arrange
        _topics.RemoveAll();
        
        // Act
        var topics = await _topics.GetAll();

        // Assert
        Assert.Empty(topics);
    }
    
    [Fact]
    public async void GetAll_SomeTopics_ReturnsNonEmptyList()
    {
        // Arrange
        _ = CreateMockTopic();
        
        // Act
        var topics = await _topics.GetAll();

        // Assert
        Assert.NotEmpty(topics);
    }

    [Fact]
    public async void GetTopicById_ValidId_ReturnsRequestedTopic()
    {
        // Arrange
        var mockTopic = await CreateMockTopic();
        
        // Act
        var foundTopic = await _topics.GetTopicById(mockTopic.Id);

        // Assert
        Assert.Equal(mockTopic.Id, foundTopic!.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-2000)]
    [InlineData(int.MinValue)]
    public async void GetTopicById_InvalidId_ReturnsNull(int invalidId)
    {
        // Act
        var foundTopic = await _topics.GetTopicById(invalidId);

        // Assert
        Assert.Null(foundTopic);
    }
    
    [Fact]
    public async void GetTopicByName_ValidName_ReturnsRequestedTopic()
    {
        // Arrange
        var mockTopic = await CreateMockTopic();

        // Act
        var foundTopic = await _topics.GetTopicByName(mockTopic.Name);

        // Assert
        Assert.Equal(mockTopic.Name, foundTopic!.Name);
    }
    
    [Theory]
    [InlineData("")]
    public async void GetTopicByName_InvalidName_ReturnsNull(string invalidName)
    {
        // Act
        var foundTopic = await _topics.GetTopicByName(invalidName);

        // Assert
        Assert.Null(foundTopic);
    }

    /// <summary>
    /// Create a mock topic in the database for testing purposes, with a randomly generated string being used for the
    /// topic name.
    /// </summary>
    /// <returns>A mock topic with a randomly generated name.</returns>
    private async Task<Topic> CreateMockTopic()
    {
        var topic = new Topic
        {
            Name = Guid.NewGuid().ToString(),
            Description = "Test"
        };
        topic.Id = await _topics.CreateTopic(topic);

        return topic;
    }
}