using System.Collections.Generic;
using Forum.Data;
using Forum.Data.Models;
using Forum.Data.Queries;
using Xunit;

namespace Data.Tests.Queries;

public class TopicTests
{
    [Fact] public async void GetAll_ReturnsIEnumerableOfTopics()
    {
        var topics = new Topics(new Database());
        var actual = await Topics.GetAll();

        Assert.IsType<IEnumerable<Topic>>(actual);
    }
}