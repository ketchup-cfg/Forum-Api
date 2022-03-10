using System;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data.Models;
using Forum.Data.Queries;
using Forum.Tests.Library.Fixtures;
using Forum.Tests.Library.Helpers;
using Npgsql;
using Xunit;

namespace Forum.Data.Tests.Queries;

public class TopicTests : IClassFixture<DatabaseFixture>
{
    private readonly Topics _topics;
    private readonly TopicHelpers _helpers;

    public TopicTests(DatabaseFixture fixture)
    {
        _topics = new Topics(fixture.Database);
        _helpers = new TopicHelpers(_topics);
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
        _ = _helpers.CreateMockTopic();
        
        // Act
        var topics = await _topics.GetAll();

        // Assert
        Assert.NotEmpty(topics);
    }

    [Fact]
    public async void GetTopicById_ValidId_ReturnsRequestedTopic()
    {
        // Arrange
        var expected = (await _helpers.CreateMockTopic()).Id;
        
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
        var expected = (await _helpers.CreateMockTopic()).Name;

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
    [InlineData("Test", "")]
    [InlineData("Testdsgsdgsd", null)]
    [InlineData("A good name for a topic", "I agree fully")]
    [InlineData("AH BEES", "🐝")]
    public async void CreateTopic_ValidNameAndValidDescription_ReturnsValidIdOfNewTopic(string validName, string validDescription)
    {
        // Arrange
        var newTopic = new Topic
        {
            Name = validName,
            Description = validDescription
        };
        
        // Act
        var newTopicId = await _topics.CreateTopic(newTopic);
        var foundTopic = await _topics.GetTopicById(newTopicId);
        
        // Assert
        Assert.NotNull(foundTopic);
    }
    
    [Theory]
    [InlineData(null, "")]
    [InlineData(null, null)]
    [InlineData(null, "I agree fully")]
    [InlineData(null, "🐝")]
    public async void CreateTopic_NullNameAndValidDescription_ThrowsNotNullException(string invalidName, string validDescription)
    {
        // Arrange
        const string expected = PostgresErrorCodes.NotNullViolation;
        var newTopic = new Topic
        {
            Name = invalidName,
            Description = validDescription
        };
        Func<Task<int>> createTopic = async () => await _topics.CreateTopic(newTopic);
        
        // Act
        var actual = (await Record.ExceptionAsync(createTopic) as PostgresException)!.SqlState;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void CreateTopic_NameAlreadyExists_ThrowsUniqueException()
    {
        // Arrange
        const string expected = PostgresErrorCodes.UniqueViolation;
        var mockTopic = await _helpers.CreateMockTopic();
        Func<Task<int>> createTopic = async () => await _topics.CreateTopic(mockTopic);
        
        // Act
        var actual = (await Record.ExceptionAsync(createTopic) as PostgresException)!.SqlState;

        // Assert
        Assert.Equal(expected, actual);
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
        var mockTopic = await _helpers.CreateMockTopic();
        mockTopic.Description = validDescription;

        // Act
        var actual = await _topics.UpdateTopic(mockTopic);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    public async void UpdateTopic_NullName_ThrowsNotNullException(string invalidName)
    {
        // Arrange
        const string expected = PostgresErrorCodes.NotNullViolation;
        var mockTopic = await _helpers.CreateMockTopic();
        mockTopic.Name = invalidName;
        Func<Task<int>> updateTopic = async () => await _topics.UpdateTopic(mockTopic);
        
        // Act
        var actual = (await Record.ExceptionAsync(updateTopic) as PostgresException)!.SqlState;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void UpdateTopic_NameAlreadyExists_ThrowsUniqueException()
    {
        // Arrange
        const string expected = PostgresErrorCodes.UniqueViolation;
        var mockTopic1 = await _helpers.CreateMockTopic();
        var mockTopic2 = await _helpers.CreateMockTopic();
        mockTopic1.Name = mockTopic2.Name;
        Func<Task<int>> updateTopic = async () => await _topics.UpdateTopic(mockTopic1);
        
        // Act
        var actual = (await Record.ExceptionAsync(updateTopic) as PostgresException)!.SqlState;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void UpdateTopic_NoChangesMade_UpdatesOneTopic()
    {
        // Arrange
        const int expected = 1;
        var mockTopic = await _helpers.CreateMockTopic();

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
}