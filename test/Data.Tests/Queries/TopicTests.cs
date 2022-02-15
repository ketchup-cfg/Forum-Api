using System.Collections.Generic;
using Data.Tests.Fixtures;
using Forum.Data.Models;
using Forum.Data.Queries;
using Xunit;

namespace Data.Tests.Queries;

public class TopicTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly Topics _topics;

    public TopicTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _topics = new Topics(_fixture.Database);
    }
    
    [Fact] public async void GetAll_ReturnsIEnumerableOfTopics()
    {
        var actual = await _topics.GetAll();

        Assert.IsAssignableFrom<IEnumerable<Topic>>(actual);
    }
}