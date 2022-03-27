using System;
using System.Linq;
using System.Threading.Tasks;
using Somewhere.Core.Exceptions;
using Somewhere.Core.Abstractions;
using Somewhere.Testing.Mocks;
using Xunit;

namespace Somewhere.Core.Tests.Services;

public class TopicServiceTests
{
    private readonly ITopicsService _topics;

    public TopicServiceTests()
    {
        var mockTopicsService = MockTopicsService.GenerateMockTopicsService();
        _topics = mockTopicsService.Object;
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
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(40)]
    [InlineData(60)]
    public async void GetTopic_ById_TopicDoesExist_ReturnsRequestedTopic(int id)
    {
        // Arrange
        var expected = id;
        // Act
        var actual = (await _topics.GetTopic(id))!.Id;

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
    [InlineData(100)]
    [InlineData(2000)]
    [InlineData(int.MaxValue)]
    public async void GetTopic_ById_TopicDoesNotExist_ReturnsNull(int id)
    {
        // Act
        var topic = await _topics.GetTopic(id);

        // Assert
        Assert.Null(topic);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("5")]
    [InlineData("10")]
    [InlineData("40")]
    [InlineData("60")]
    public async void GetTopic_ByName_TopicDoesExist_ReturnsRequestedTopic(string name)
    {
        // Arrange
        var expected = name;

        // Act
        var actual = (await _topics.GetTopic(expected))!.Name;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("-2000")]
    [InlineData("-100")]
    [InlineData("-10")]
    [InlineData("-2")]
    [InlineData("-1")]
    [InlineData("100")]
    [InlineData("2000")]
    public async void GetTopic_ByName_TopicDoesNotExist_ReturnsNull(string name)
    {
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
    public async void AddTopic_ValidNameAndValidDescription_ReturnsNewTopic(string name, string description)
    {
        // Arrange
        var mockTopic = MockTopic.Create(name: name, description: description);

        // Act
        var createdTopic = await _topics.AddTopic(mockTopic);

        // Assert
        Assert.Equal(mockTopic.Name, createdTopic.Name);
        Assert.Equal(mockTopic.Description, createdTopic.Description);
    }

    [Theory]
    [InlineData("0", "")]
    [InlineData("1", null)]
    [InlineData("2", "I agree fully")]
    [InlineData("3", "🐝")]
    public async void AddTopic_NameAlreadyExists_ThrowsDuplicateTopicNameException(string name, string? description)
    {
        // Arrange
        var topic = MockTopic.Create(name: name, description: description);

        // Act
        Func<Task> addTopic = async () => _ = await _topics.AddTopic(topic);

        // Assert
        await Assert.ThrowsAsync<DuplicateTopicNameException>(addTopic);
    }

    [Theory]
    [InlineData(0, "Test")]
    [InlineData(1, "Elbows")]
    [InlineData(5, "fdsfds")]
    [InlineData(10, "Just your average valid topic description")]
    [InlineData(40, "")]
    [InlineData(60, null)]
    public async void UpdateTopic_ValidIdAndValidDescription_UpdatesOneTopic(int id, string description)
    {
        // Arrange
        const int expected = 1;
        var topic = await _topics.GetTopic(id);

        // Act
        topic!.Description = description;
        var actual = await _topics.UpdateTopic(id, topic);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "0")]
    [InlineData(5, "3")]
    [InlineData(10, "5")]
    [InlineData(40, "10")]
    [InlineData(60, "40")]
    public async void UpdateTopic_NameAlreadyExists_ThrowsDuplicateNameException(int id, string name)
    {
        // Arrange
        var topic = await _topics.GetTopic(id);

        // Act
        topic!.Name = name;
        Func<Task> updateTopic = async () => _ = await _topics.UpdateTopic(id, topic);

        // Assert
        await Assert.ThrowsAsync<DuplicateTopicNameException>(updateTopic);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(40)]
    [InlineData(60)]
    public async void UpdateTopic_NoChangesMade_UpdatesOneTopic(int id)
    {
        // Arrange
        const int expected = 1;
        var topic = await _topics.GetTopic(id);

        // Act
        var actual = await _topics.UpdateTopic(id, topic!);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(40)]
    [InlineData(60)]
    public async void RemoveTopic_TopicExists_RemovesOneTopic(int id)
    {
        // Arrange
        const int expected = 1;

        // Act
        var actual = await _topics.RemoveTopic(id);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    [InlineData(-100)]
    [InlineData(-2000)]
    [InlineData(int.MinValue)]
    public async void RemoveTopic_TopicDoesNotExist_RemovesZeroTopics(int id)
    {
        // Arrange
        const int expected = 0;

        // Act
        var actual = await _topics.RemoveTopic(id);

        // Assert
        Assert.Equal(expected, actual);
    }
}