using System.Text.RegularExpressions;
using Moq;
using Somewhere.Data.Models;
using Somewhere.Core.Abstractions;
using Somewhere.Core.Exceptions;
using Range = Moq.Range;

namespace Somewhere.Testing.Mocks;

public static class MockTopicsService
{
    private static readonly IEnumerable<Topic> Topics = MockTopic.CreateMany(MockTopic.MaxTopicId);

    private const string TwoPositiveDigitsPattern = @"^\d{1,2}$";
    private const string NotTwoPositiveDigitsPattern = @"^.*(?<!\d{1,2})(?!\d{1,2}).*$";

    private const int SuccessfulNumberOfUpdates = 1;
    private const int UnsuccessfulNumberOfUpdates = 0;

    private const int SuccessfulNumberOfDeletes = 1;
    private const int UnsuccessfulNumberOfDeletes = 0;

    public static Mock<ITopicsService> GenerateMockTopicsService()
    {
        var mock = new Mock<ITopicsService>();

        /*
         * Setup GetAllTopics to return the requested number of topics if called with a limit between 0 and 30.
         */
        mock.Setup(service =>
                service.GetAllTopics(
                        It.IsInRange(MockTopic.MinTopicId, MockTopic.MaxTopicsPerPage, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns((int limit, int _) => Topics.Take(limit));

        /*
         * Setup GetAllTopics to return 30 topics if called with a limit that is greater than 30.
         */
        mock.Setup(service =>
                service.GetAllTopics(
                        It.IsInRange(MockTopic.MaxTopicsPerPage, int.MaxValue, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns(Topics.Take(MockTopic.MaxTopicsPerPage));

        /*
         * Setup GetAllTopics to return an empty list if called with a negative limit.
         */
        mock.Setup(service =>
                service.GetAllTopics(
                        It.IsInRange(int.MinValue, -1, Range.Inclusive),
                        It.IsAny<int>())
                    .Result)
            .Returns(Topics.Take(0));

        /*
         * Setup GetTopic to return the specified topic if called with a valid ID.
         */
        mock.Setup(service =>
                service.GetTopic(It.IsInRange(MockTopic.MinTopicId, MockTopic.MaxTopicId, Range.Inclusive))
                    .Result)
            .Returns((int id) => MockTopic.Create(id));

        /*
         * Setup GetTopic to return a null topic reference if called with an invalid ID that is lower that the minimum
         * topic ID value.
         */
        mock.Setup(service =>
                service.GetTopic(It.IsInRange(int.MinValue, MockTopic.MinTopicId - 1, Range.Inclusive)))
            .ReturnsAsync((Topic?) null);

        /*
         * Setup GetTopic to return a null topic reference if called with an invalid ID that is higher that the maximum
         * topic ID value.
         */
        mock.Setup(service =>
                service.GetTopic(It.IsInRange(MockTopic.MaxTopicId + 1, int.MaxValue, Range.Inclusive)))
            .ReturnsAsync((Topic?) null);

        /*
         * Setup GetTopic to return the specified topic if called with a valid name.
         */
        mock.Setup(service =>
                service.GetTopic(It.IsRegex(TwoPositiveDigitsPattern)))
            .ReturnsAsync((string name) => MockTopic.Create(name: name));

        /*
         * Setup GetTopic to return a null topic reference if called with an invalid name.
         */
        mock.Setup(service =>
                service.GetTopic(It.IsRegex(NotTwoPositiveDigitsPattern)))
            .ReturnsAsync((Topic?) null);

        /*
         * Setup AddTopic to return a new topic containing the specified topic name and description.
         *
         * If the suggested name is just two positive digits, throw a DuplicateTopicNameException exception.
         */
        mock.Setup(service =>
                service.AddTopic(It.IsAny<Topic>())
                    .Result)
            .Returns((Topic newTopic) =>
            {
                if (Regex.IsMatch(newTopic.Name, TwoPositiveDigitsPattern))
                {
                    throw new DuplicateTopicNameException();
                }

                return MockTopic.Create(1, newTopic.Name, newTopic.Description);
            });

        /*
         * Setup UpdateTopic to return the number 1 if a valid topic is passed in.
         *
         * Throw a DuplicateIdException exception if the new ID is not the same and is between 0 and 99.
         *
         * Throw a DuplicateTopicNameException exception if the new name is not the same and is just two positive digits.
         */
        mock.Setup(service =>
                service.UpdateTopic(
                        It.IsInRange(MockTopic.MinTopicId, MockTopic.MaxTopicId, Range.Inclusive),
                        It.IsAny<Topic>())
                    .Result)
            .Returns((int id, Topic updatedTopic) =>
            {
                if (id != updatedTopic.Id && updatedTopic.Id is >= MockTopic.MinTopicId and <= MockTopic.MaxTopicId)
                {
                    throw new DuplicateIdException();
                }

                if (id.ToString() != updatedTopic.Name && Regex.IsMatch(updatedTopic.Name, TwoPositiveDigitsPattern))
                {
                    throw new DuplicateTopicNameException();
                }

                return SuccessfulNumberOfUpdates;
            });

        /*
         * Setup UpdateTopic to return the number 0 if a topic is passed in for an ID that does not exist.
         */
        mock.Setup(service =>
                service.UpdateTopic(
                        It.IsInRange(int.MinValue, MockTopic.MinTopicId - 1, Range.Inclusive),
                        It.IsAny<Topic>())
                    .Result)
            .Returns(UnsuccessfulNumberOfUpdates);

        /*
         * Setup UpdateTopic to return the number 0 if a topic is passed in for an ID that does not exist.
         */
        mock.Setup(service =>
                service.UpdateTopic(
                        It.IsInRange(MockTopic.MaxTopicId + 1, int.MaxValue, Range.Inclusive),
                        It.IsAny<Topic>())
                    .Result)
            .Returns(UnsuccessfulNumberOfUpdates);

        /*
         * Setup RemoveTopic to return the number 1 if a valid topic ID is passed in.
         */
        mock.Setup(service =>
                service.RemoveTopic(It.IsInRange(0, MockTopic.MaxTopicId, Range.Inclusive))
                    .Result)
            .Returns(SuccessfulNumberOfDeletes);

        /*
         * Setup RemoveTopic to return the number 0 if an invalid topic ID is passed in.
         */
        mock.Setup(service =>
                service.RemoveTopic(It.IsInRange(int.MinValue, MockTopic.MinTopicId - 1, Range.Inclusive))
                    .Result)
            .Returns(UnsuccessfulNumberOfDeletes);

        /*
         * Setup RemoveTopic to return the number 0 if an invalid topic ID is passed in.
         */
        mock.Setup(service =>
                service.RemoveTopic(It.IsInRange(MockTopic.MaxTopicId + 1, int.MaxValue, Range.Inclusive))
                    .Result)
            .Returns(UnsuccessfulNumberOfDeletes);

        return mock;
    }
}