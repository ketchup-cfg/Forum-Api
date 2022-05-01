using System;
using System.Linq;
using System.Threading.Tasks;
using HeavyMetalMachine.Core.Abstractions;
using HeavyMetalMachine.Testing.Mocks;
using Xunit;

namespace HeavyMetalMachine.Core.Tests.Services;

public class PostsServiceTests
{
    private readonly IPostsService _posts;

    public PostsServiceTests()
    {
        var mockPostsService = MockPostsService.GenerateMockPostsService();
        _posts = mockPostsService.Object;
    }

    [Fact]
    public async void GetAllPosts_DefaultLimitAndOffset_Returns30Posts()
    {
        // Arrange
        const int expected = 30;

        // Act
        var actual = (await _posts.GetAllPosts()).Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(30)]
    public async void GetAllPosts_LimitBetween0And30_ReturnsRequestedNumberOfPosts(int numberOfPosts)
    {
        // Arrange
        var expected = numberOfPosts;

        // Act
        var actual = (await _posts.GetAllPosts(numberOfPosts)).Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-20)]
    [InlineData(-50)]
    [InlineData(int.MinValue)]
    public async void GetAllPosts_NegativeLimit_ReturnsNoPosts(int numberOfPosts)
    {
        // Arrange
        const int expected = 0;

        // Act
        var actual = (await _posts.GetAllPosts(numberOfPosts)).Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(31)]
    [InlineData(40)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public async void GetAllPosts_LimitAbove30_Returns30Posts(int numberOfPosts)
    {
        // Arrange
        const int expected = 30;

        // Act
        var actual = (await _posts.GetAllPosts(numberOfPosts)).Count();

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
    public async void GetPost_PostDoesExist_ReturnsRequestedPost(int id)
    {
        // Arrange
        var expected = id;
        // Act
        var actual = (await _posts.GetPost(id))!.Id;

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
    public async void GetPost_PostDoesNotExist_ReturnsNull(int id)
    {
        // Act
        var post = await _posts.GetPost(id);

        // Assert
        Assert.Null(post);
    }

    [Theory]
    [InlineData("Test", "")]
    [InlineData("Testdsgsdgsd", null)]
    [InlineData("A good title for a post", "I agree fully")]
    [InlineData("AH BEES", "🐝")]
    public async void AddPost_ValidTitleAndValidContent_ReturnsNewPost(string title, string content)
    {
        // Arrange
        var mockPost = MockPost.Create(title: title, content: content);

        // Act
        var createdPost = await _posts.AddPost(mockPost);

        // Assert
        Assert.Equal(mockPost.Title, createdPost.Title);
        Assert.Equal(mockPost.Content, createdPost.Content);
    }

    [Theory]
    [InlineData(0, "Test", "Test")]
    [InlineData(1, "Elbows", "Elbows")]
    [InlineData(5, "fdsfds", "fdsfds")]
    [InlineData(10, "Just your average valid post title", "Just your average valid post content")]
    [InlineData(40, "", "")]
    [InlineData(60, null, null)]
    public async void UpdatePost_ValidIdAndValidData_UpdatesOnePost(int id, string title, string content)
    {
        // Arrange
        const int expected = 1;
        var post = await _posts.GetPost(id);

        // Act
        post!.Title = title;
        post!.Content = content;
        var actual = await _posts.UpdatePost(id, post);

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
    public async void UpdatePost_NoChangesMade_UpdatesOnePost(int id)
    {
        // Arrange
        const int expected = 1;
        var post = await _posts.GetPost(id);

        // Act
        var actual = await _posts.UpdatePost(id, post!);

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
    public async void RemovePost_PostExists_RemovesOnePost(int id)
    {
        // Arrange
        const int expected = 1;

        // Act
        var actual = await _posts.RemovePost(id);

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
    public async void RemovePost_PostDoesNotExist_RemovesZeroPost(int id)
    {
        // Arrange
        const int expected = 0;

        // Act
        var actual = await _posts.RemovePost(id);

        // Assert
        Assert.Equal(expected, actual);
    }
}