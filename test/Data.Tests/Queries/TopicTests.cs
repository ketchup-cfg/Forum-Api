using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Tests.Fixtures;
using Forum.Data.Models;
using Forum.Data.Queries;
using Xunit;

namespace Data.Tests.Queries;

public class TopicTests : IClassFixture<DatabaseFixture>
{
    private readonly Topics _topics;

    public TopicTests(DatabaseFixture fixture)
    {
        _topics = new Topics(fixture.Database);
    }
    
    [Fact]
    public async void GetAll_ReturnsIEnumerableOfTopics()
    {
        var actual = await _topics.GetAll();

        Assert.IsAssignableFrom<IEnumerable<Topic>>(actual);
    }
    
    [Fact]
    public async void GetTopicById_ValidId_ReturnsRequestedTopic()
    {
        var topics = await _topics.GetAll();
        var validTopic = topics.First();
        var foundTopic = await _topics.GetTopicById(validTopic.Id);

        Assert.NotNull(foundTopic);
    }
    
    [Fact]
    public async void GetTopicById_InvalidId_ReturnsNull()
    {
        const int invalidId = -1;
        var foundTopic = await _topics.GetTopicById(invalidId);

        Assert.Null(foundTopic);
    }
    
    [Fact]
    public async void GetTopicByName_ValidName_ReturnsRequestedTopic()
    {
        var topics = await _topics.GetAll();
        var validTopic = topics.First();
        var foundTopic = await _topics.GetTopicByName(validTopic.Name);

        Assert.NotNull(foundTopic);
    }
    
    [Fact]
    public async void GetTopicByName_InvalidName_ReturnsNull()
    {
        var invalidName = string.Empty;
        var foundTopic = await _topics.GetTopicByName(invalidName);

        Assert.Null(foundTopic);
    }
}