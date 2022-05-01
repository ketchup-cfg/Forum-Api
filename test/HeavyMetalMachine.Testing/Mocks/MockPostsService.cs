using Moq;
using HeavyMetalMachine.Core.Abstractions;
using HeavyMetalMachine.Core.Models;
using Range = Moq.Range;

namespace HeavyMetalMachine.Testing.Mocks;

public static class MockPostsService
{
    private static readonly IEnumerable<Post> Posts = MockPost.CreateMany(MockPost.MaxPostId);

    private const int SuccessfulNumberOfUpdates = 1;
    private const int UnsuccessfulNumberOfUpdates = 0;

    private const int SuccessfulNumberOfDeletes = 1;
    private const int UnsuccessfulNumberOfDeletes = 0;

    public static Mock<IPostsService> GenerateMockPostsService()
    {
        var mock = new Mock<IPostsService>();

        /*
         * Setup GetAllPosts to return the requested number of posts if called with a limit between 0 and 30.
         */
        mock.Setup(service =>
                service.GetAllPosts(
                        It.IsInRange(MockPost.MinPostId, MockPost.MaxPostsPerPage, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns((int limit, int _) => Posts.Take(limit));

        /*
         * Setup GetAllPosts to return 30 posts if called with a limit that is greater than 30.
         */
        mock.Setup(service =>
                service.GetAllPosts(
                        It.IsInRange(MockPost.MaxPostsPerPage, int.MaxValue, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns(Posts.Take(MockPost.MaxPostsPerPage));

        /*
         * Setup GetAllPosts to return an empty list if called with a negative limit.
         */
        mock.Setup(service =>
                service.GetAllPosts(
                        It.IsInRange(int.MinValue, -1, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns(Posts.Take(0));

        /*
         * Setup GetPost to return the specified post if called with a valid ID.
         */
        mock.Setup(service =>
                service.GetPost(It.IsInRange(MockPost.MinPostId, MockPost.MaxPostId, Range.Inclusive))
                    .Result)
            .Returns((int id) => MockPost.Create(id));

        /*
         * Setup GetPost to return a null post reference if called with an invalid ID that is lower that the minimum
         * post ID value.
         */
        mock.Setup(service =>
                service.GetPost(It.IsInRange(int.MinValue, MockPost.MinPostId - 1, Range.Inclusive)))
            .ReturnsAsync((Post?) null);

        /*
         * Setup GetPost to return a null post reference if called with an invalid ID that is higher that the maximum
         * post ID value.
         */
        mock.Setup(service =>
                service.GetPost(It.IsInRange(MockPost.MaxPostId + 1, int.MaxValue, Range.Inclusive)))
            .ReturnsAsync((Post?) null);

        /*
         * Setup AddPost to return a new post containing the specified post title and content.
         */
        mock.Setup(service =>
                service.AddPost(It.IsAny<Post>())
                    .Result)
            .Returns((Post newPost) => MockPost.Create(1, newPost.Title, newPost.Content));

        /*
         * Setup UpdatePost to return the number 1 if a valid post is passed in.
         */
        mock.Setup(service =>
                service.UpdatePost(
                        It.IsInRange(MockPost.MinPostId, MockPost.MaxPostId, Range.Inclusive),
                        It.IsAny<Post>())
                    .Result)
            .Returns((int id, Post updatedPost) => SuccessfulNumberOfUpdates);

        /*
         * Setup UpdatePost to return the number 0 if a post is passed in for an ID that does not exist.
         */
        mock.Setup(service =>
                service.UpdatePost(
                        It.IsInRange(int.MinValue, MockPost.MinPostId - 1, Range.Inclusive),
                        It.IsAny<Post>())
                    .Result)
            .Returns(UnsuccessfulNumberOfUpdates);

        /*
         * Setup UpdatePost to return the number 0 if a post is passed in for an ID that does not exist.
         */
        mock.Setup(service =>
                service.UpdatePost(
                        It.IsInRange(MockPost.MaxPostId + 1, int.MaxValue, Range.Inclusive),
                        It.IsAny<Post>())
                    .Result)
            .Returns(UnsuccessfulNumberOfUpdates);

        /*
         * Setup RemovePost to return the number 1 if a valid post ID is passed in.
         */
        mock.Setup(service =>
                service.RemovePost(It.IsInRange(0, MockPost.MaxPostId, Range.Inclusive))
                    .Result)
            .Returns(SuccessfulNumberOfDeletes);

        /*
         * Setup RemovePost to return the number 0 if an invalid post ID is passed in.
         */
        mock.Setup(service =>
                service.RemovePost(It.IsInRange(int.MinValue, MockPost.MinPostId - 1, Range.Inclusive))
                    .Result)
            .Returns(UnsuccessfulNumberOfDeletes);

        /*
         * Setup RemovePost to return the number 0 if an invalid post ID is passed in.
         */
        mock.Setup(service =>
                service.RemovePost(It.IsInRange(MockPost.MaxPostId + 1, int.MaxValue, Range.Inclusive))
                    .Result)
            .Returns(UnsuccessfulNumberOfDeletes);

        return mock;
    }
}