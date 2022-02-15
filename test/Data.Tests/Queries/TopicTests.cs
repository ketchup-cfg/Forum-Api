using System.Collections.Generic;
using Forum.Data.Models;
using Forum.Data.Queries;
using Xunit;

namespace Data.Tests.Queries;

public class TopicTests
{
    [Fact] public void GetAll_ReturnsListOfTopics()
    {
        var actual = Topics.GetAll();

        Assert.IsType<List<Topic>>(actual);
    }
}