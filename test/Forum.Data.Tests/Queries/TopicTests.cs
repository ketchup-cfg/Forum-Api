using System;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data.Models;
using Forum.Data.Queries;
using Forum.Tests.Library.Fixtures;
using Npgsql;
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
        _ = await _topics.RemoveAll();
        
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
        var expected = (await CreateMockTopic()).Id;
        
        // Act
        var actual = (await _topics.GetTopicById(expected))!.Id;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-2000)]
    [InlineData(int.MinValue)]
    public async void GetTopicById_TopicDoesNotExist_ReturnsNull(int invalidId)
    {
        // Arrange
        _ = await _topics.RemoveAll();
        
        // Act
        var foundTopic = await _topics.GetTopicById(invalidId);

        // Assert
        Assert.Null(foundTopic);
    }
    
    [Fact]
    public async void GetTopicByName_ValidName_ReturnsRequestedTopic()
    {
        // Arrange
        var expected = (await CreateMockTopic()).Name;

        // Act
        var actual = (await _topics.GetTopicByName(expected))!.Name;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("Test Topic")]
    [InlineData("")]
    [InlineData(null)]
    public async void GetTopicByName_TopicDoesNotExist_ReturnsNull(string invalidName)
    {
        // Arrange
        _ = await _topics.RemoveAll();
        
        // Act
        var foundTopic = await _topics.GetTopicByName(invalidName);

        // Assert
        Assert.Null(foundTopic);
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("Elbows")]
    [InlineData("fdsfds")]
    [InlineData("Just your average valid topic description")]
    [InlineData("")]
    [InlineData(null)]
    public async void UpdateTopic_ValidDescription_UpdatesOneTopic(string validDescription)
    {
        // Arrange
        const int expected = 1;
        var mockTopic = await CreateMockTopic();
        mockTopic.Description = validDescription;

        // Act
        var actual = await _topics.UpdateTopic(mockTopic);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    public async void UpdateTopic_InvalidName_ThrowsException(string invalidName)
    {
        // Arrange
        var mockTopic = await CreateMockTopic();
        mockTopic.Name = invalidName;
        Func<Task<int>> updateTopic = async () => await _topics.UpdateTopic(mockTopic);
        
        // Act
        var exception = await Record.ExceptionAsync(updateTopic);
        
        // Assert
        Assert.IsType<Npgsql.PostgresException>(exception);
    }

    [Fact]
    public async void UpdateTopic_NameAlreadyExists_ThrowsException()
    {
        // Arrange
        var mockTopic1 = await CreateMockTopic();
        var mockTopic2 = await CreateMockTopic();
        mockTopic1.Name = mockTopic2.Name;
        Func<Task<int>> updateTopic = async () => await _topics.UpdateTopic(mockTopic1);
        
        // Act
        var exception = await Record.ExceptionAsync(updateTopic);

        // Assert
        Assert.IsType<Npgsql.PostgresException>(exception);
    }
    
    [Fact]
    public async void UpdateTopic_NoChangesMade_UpdatesOneTopic()
    {
        // Arrange
        const int expected = 1;
        var mockTopic = await CreateMockTopic();

        // Act
        var actual = await _topics.UpdateTopic(mockTopic);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void RemoveAll_AnySituation_RemovesAllTopics()
    {
        // Arrange
        var expected = (await _topics.GetAll()).Count();
        
        // Act
        var actual = await _topics.RemoveAll();

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Create a mock topic in the database for testing purposes, with a randomly generated string being used for the
    /// topic name.
    /// </summary>
    /// <returns>A mock topic with a randomly generated name.</returns>
    private async Task<Topic> CreateMockTopic(string description = "Test")
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