using System.Collections.Generic;
using Forum.Data;
using Forum.Data.Models;
using Forum.Data.Queries;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Data.Tests.Queries;

public class TopicTests
{
    private readonly IConfiguration _config;

    public TopicTests()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("../appsettings.Development.json")
            .Build();
    }
    [Fact] public async void GetAll_ReturnsIEnumerableOfTopics()
    {
        var topics = new Topics(new Database(_config.GetConnectionString("Test")));
        var actual = await topics.GetAll();

        Assert.IsType<IEnumerable<Topic>>(actual);
    }
}