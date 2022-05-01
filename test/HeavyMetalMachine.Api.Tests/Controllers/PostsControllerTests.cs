using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using HeavyMetalMachine.Testing.Mocks;
using HeavyMetalMachine.Api.Controllers;
using Xunit;

namespace HeavyMetalMachine.Api.Tests.Controllers;

public class PostsControllerTests
{
    private readonly PostsController _controller;

    public PostsControllerTests()
    {
        var mock = MockPostsService.GenerateMockPostsService();
        var logger = Mock.Of<ILogger<PostsController>>();
        _controller = new PostsController(mock.Object, logger);
    }

    [Fact]
    public async void GetAllPosts_NoParameters_Returns200Status()
    {
        // Arrange
        const int expected = StatusCodes.Status200OK;

        // Act
        var response = await _controller.GetAllPosts();
        var actual = ((ObjectResult) response).StatusCode;

        // Assert
        Assert.Equal(expected, actual);
    }
}