using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Somewhere.Api.Controllers;
using Somewhere.Testing.Mocks;
using Xunit;

namespace Somewhere.Api.Tests.Controllers;

public class TopicsControllerTests
{
    private readonly TopicsController _controller;
    
    public TopicsControllerTests()
    {
        
        var mock = MockTopicsService.GenerateMockTopicsService();
        _controller = new TopicsController(mock.Object);
    }
    
    [Fact]
    public async void GetAllTopics_NoParameters_Returns200Status()
    {
        // Arrange
        const int expected = StatusCodes.Status200OK;
        
        // Act
        var response = await _controller.GetAllTopics();
        var actual = ((ObjectResult) response).StatusCode;
        
        // Assert
        Assert.Equal(expected, actual);
    }
}