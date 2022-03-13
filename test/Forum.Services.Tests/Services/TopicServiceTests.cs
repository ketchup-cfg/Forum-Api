using System;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data.Models;
using Forum.Services.Exceptions;
using Forum.Services.Services;
using Forum.Tests.Library.Fixtures;
using Forum.Tests.Library.Helpers;
using Npgsql;
using Xunit;

namespace Forum.Services.Tests.Services;

public class TopicServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly TopicsService _topics;
    private readonly TopicHelpers _helpers;

    public TopicServiceTests(DatabaseFixture fixture)
    {
        _topics = new TopicsService(fixture.Database);
        _helpers = new TopicHelpers(_topics);
        _helpers.EnsureSetNumberOfTopicsExist(100);
    }

    [Fact]
    public async void GetAll_DefaultLimitAndOffset_Returns30Topics()
    {
        // Arrange
        const int expected = 30;

        // Act
        var actual = (await _topics.GetAllTopics()).Count();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(30)]
    public async void GetAll_LimitBetween0And30_ReturnsRequestedNumberOfTopics(int numberOfTopics)
    {
        // Arrange
        var expected = numberOfTopics;

        // Act
        var actual = (await _topics.GetAllTopics(numberOfTopics)).Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-20)]
    [InlineData(-50)]
    [InlineData(int.MinValue)]
    public async void GetAll_NegativeLimit_ReturnsNoTopics(int numberOfTopics)
    {
        // Arrange
        const int expected = 0;

        // Act
        var actual = (await _topics.GetAllTopics(numberOfTopics)).Count();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(31)]
    [InlineData(40)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public async void GetAll_LimitAbove30_Returns30Topics(int numberOfTopics)
    {
        // Arrange
        const int expected = 30;

        // Act
        var actual = (await _topics.GetAllTopics(numberOfTopics)).Count();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-2000)]
    [InlineData(-100)]
    [InlineData(-10)]
    [InlineData(-2)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(2000)]
    [InlineData(int.MaxValue)]
    public async void GetTopic_ById_TopicDoesNotExist_ReturnsNull(int id)
    {
        // Arrange
        await _topics.RemoveTopic(id);
        
        // Act
        var topic = await _topics.GetTopic(id);

        // Assert
        Assert.Null(topic);
    }
    
    [Fact]
    public async void GetTopic_ByName_TopicDoesExist_ReturnsRequestedTopic()
    {
        // Arrange
        var expected = (await _helpers.CreateMockTopic()).Name;

        // Act
        var actual = (await _topics.GetTopic(expected))!.Name;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("Test Topic")]
    [InlineData("")]
    [InlineData(null)]
    public async void GetTopic_ByName_TopicDoesNotExist_ReturnsNull(string name)
    {
        // Arrange
        var mockTopic = await _helpers.CreateMockTopic(name);
        await _topics.RemoveTopic(mockTopic.Id);
        
        // Act
        var foundTopic = await _topics.GetTopic(name);

        // Assert
        Assert.Null(foundTopic);
    }

    [Theory]
    [InlineData("Test", "")]
    [InlineData("Testdsgsdgsd", null)]
    [InlineData("A good name for a topic", "I agree fully")]
    [InlineData("AH BEES", "🐝")]
    public async void AddTopic_ValidNameAndValidDescription_ReturnsNewTopic(string validName, string validDescription)
    {
        // Arrange
        var newTopic = new Topic
        {
            Name = validName,
            Description = validDescription
        };
        
        // Act
        var createdTopic = await _topics.AddTopic(newTopic);

        // Assert
        Assert.Equal(newTopic.Name, createdTopic.Name);
        Assert.Equal(newTopic.Description, createdTopic.Description);
    }
    
    [Theory]
    [InlineData(null, "")]
    [InlineData(null, null)]
    [InlineData(null, "I agree fully")]
    [InlineData(null, "🐝")]
    public async void AddTopic_NullNameAndValidDescription_ThrowsNotNullException(string invalidName, string validDescription)
    {
        // Arrange
        const string expected = PostgresErrorCodes.NotNullViolation;
        var newTopic = new Topic
        {
            Name = invalidName,
            Description = validDescription
        };
        Func<Task<Topic>> createTopic = async () => await _topics.AddTopic(newTopic);
        
        // Act
        var actual = (await Record.ExceptionAsync(createTopic) as PostgresException)!.SqlState;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void AddTopic_NameAlreadyExists_ThrowsUniqueException()
    {
        // Arrange
        const string expected = PostgresErrorCodes.UniqueViolation;
        var mockTopic = await _helpers.CreateMockTopic();
        Func<Task<Topic>> createTopic = async () => await _topics.AddTopic(mockTopic);
        
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
    public async void UpdateTopic_ValidDescription_UpdatesPersist(string validDescription)
    {
        // Arrange
        var expected = await _helpers.CreateMockTopic();
        expected.Description = validDescription;

        // Act
         await _topics.UpdateTopic(expected.Id, expected);
         var actual = await _topics.GetTopic(expected.Id);
        
         // Assert
        Assert.Equal(expected.Description, actual?.Description);
    }

    [Theory]
    [InlineData(null)]
    public async void UpdateTopic_NullName_ThrowsNullTopicNameException(string invalidName)
    {
        // Arrange
        var mockTopic = await _helpers.CreateMockTopic();

        // Act
        mockTopic.Name = invalidName;
        Func<Task> updateTopic = async () => await _topics.UpdateTopic(mockTopic.Id, mockTopic);

        // Assert
        await Assert.ThrowsAsync<NullTopicNameException>(updateTopic);
    }

    [Fact]
    public async void UpdateTopic_NameAlreadyExists_ThrowsDuplicateNameException()
    {
        // Arrange
        var mockTopic1 = await _helpers.CreateMockTopic();
        var mockTopic2 = await _helpers.CreateMockTopic();

        // Act
        mockTopic1.Name = mockTopic2.Name;
        Func<Task> updateTopic = async () => await _topics.UpdateTopic(mockTopic1.Id, mockTopic1);

        // Assert
        await Assert.ThrowsAsync<DuplicateTopicNameException>(updateTopic);
    }
    
    [Fact]
    public async void UpdateTopic_NoChangesMade_ChangesPersist()
    {
        // Arrange
        var expected = await _helpers.CreateMockTopic();

        // Act
        await _topics.UpdateTopic(expected.Id, expected);
        var actual = await _topics.GetTopic(expected.Id);

        // Assert
        Assert.Equal(expected.Name, actual?.Name);
        Assert.Equal(expected.Description, actual?.Description);
    }

    [Fact]
    public async void RemoveTopic_TopicExists_TopicCannotBeFound()
    {
        // Arrange
        var mockTopic = await _helpers.CreateMockTopic();
        
        // Act
        await _topics.RemoveTopic(mockTopic.Id);
        var deletedTopic = await _topics.GetTopic(mockTopic.Id);
        
        // Assert
        Assert.Null(deletedTopic);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-2000)]
    [InlineData(int.MinValue)]
    public async void RemoveTopic_InvalidId_TopicCannotBeFound(int invalidId)
    {
        // Act
        await _topics.RemoveTopic(invalidId);
        var deletedTopic = await _topics.GetTopic(invalidId);
        
        // Assert
        Assert.Null(deletedTopic);
    }

    [Fact]
    public async void TopicExists_ById_TopicActuallyExists_ReturnsTrue()
    {
        // Arrange
        const bool expected = true;
        var topic = (await _topics.GetAllTopics()).First();
        
        // Act
        var actual = await _topics.TopicExists(topic.Id);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void TopicExists_ById_TopicDoesNotExist_ReturnsFalse()
    {
        // Arrange
        const bool expected = false;
        var topic = (await _topics.GetAllTopics()).First();
        await _topics.RemoveTopic(topic.Id);
        
        // Act
        var actual = await _topics.TopicExists(topic.Id);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void TopicExists_ByName_TopicActuallyExists_ReturnsTrue()
    {
        // Arrange
        const bool expected = true;
        var topic = (await _topics.GetAllTopics()).First();
        
        // Act
        var actual = await _topics.TopicExists(topic.Name);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void TopicExists_ByName_TopicDoesNotExist_ReturnsFalse()
    {
        // Arrange
        const bool expected = false;
        var topic = (await _topics.GetAllTopics()).First();
        await _topics.RemoveTopic(topic.Id);
        
        // Act
        var actual = await _topics.TopicExists(topic.Name);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void NameIsUnique_NameIsActuallyUnique_ReturnsTrue()
    {
        // Arrange
        const bool expected = true;
        var name = Guid.NewGuid().ToString();
        
        // Act
        var actual = await _topics.NameIsUnique(name);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void NameIsUnique_NameIsNotUnique_ReturnsFalse()
    {
        // Arrange
        const bool expected = false;
        var topic = (await _topics.GetAllTopics()).First();
        
        // Act
        var actual = await _topics.NameIsUnique(topic.Name);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void NewNameIsUnique_NameIsUnique_ReturnsTrue()
    {
        // Arrange
        const bool expected = true;
        var topic = (await _topics.GetAllTopics()).First();
        topic.Name = Guid.NewGuid().ToString();
        
        // Act
        var actual = await _topics.NewNameIsUnique(topic.Id, topic.Name);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void NewNameIsUnique_NameIsNotUnique_ReturnsFalse()
    {
        // Arrange
        const bool expected = false;
        var topic = (await _topics.GetAllTopics()).First();
        topic.Name = (await _topics.GetAllTopics()).Last().Name;

        // Act
        var actual = await _topics.NewNameIsUnique(topic.Id, topic.Name);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void NewIdIsUnique_IdIsUnique_ReturnsTrue()
    {
        // Arrange
        const bool expected = true;
        var topic = (await _topics.GetAllTopics()).First();
        var topicId = topic.Id;
        topic.Id = (await _topics.GetAllTopics()).Last().Id;
        await _topics.RemoveTopic(topic.Id);
        
        // Act
        var actual = await _topics.NewIdIsUnique(topicId, topic.Id);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async void NewIdIsUnique_IdIsNotUnique_ReturnsFalse()
    {
        // Arrange
        const bool expected = false;
        var topic = (await _topics.GetAllTopics()).First();
        var topicId = topic.Id;
        topic.Id = (await _topics.GetAllTopics()).Last().Id;

        // Act
        var actual = await _topics.NewIdIsUnique(topicId, topic.Id);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}