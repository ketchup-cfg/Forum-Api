using Microsoft.AspNetCore.Mvc;
using Somewhere.Data.Models;
using Somewhere.Core.Abstractions;
using Somewhere.Api.Controllers.Attributes;
using Somewhere.Api.Controllers.Base;
using Somewhere.Core.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Somewhere.Api.Controllers;

public class TopicsController : SomeBaseController
{
    private readonly ITopicsService _topics;
    private readonly ILogger<TopicsController> _log;

    public TopicsController(ITopicsService topics, ILogger<TopicsController> log)
    {
        _topics = topics;
        _log = log;
    }

    /// <summary>
    /// Find and return a collection of all existing forum topics.
    /// </summary>
    /// <returns>A collection containing data for all existing forum topics.</returns>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "A collection of topics.", typeof(IEnumerable<Topic>))]
    [SwaggerOperation(
        Summary = "Get a collection of topics",
        Description = "Returns a collection of topics.",
        OperationId = "GetAllTopics",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> GetAllTopics()
    {
        _log.LogInformation("Searching for topics");
        
        var topics = await _topics.GetAllTopics();

        _log.LogInformation("Found {TopicCount} topics", topics.Count());
        
        return Ok(topics);
    }

    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided ID.</returns>
    [HttpGet("{id:int}")]
    [SwaggerResponse(StatusCodes.Status200OK, "The requested topic was found.", typeof(Topic))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No topic was found that matches the provided topic ID.")]
    [SwaggerOperation(
        Summary = "Get a single topic by ID",
        Description = "Returns a single topic matching the provided ID.",
        OperationId = "GetTopicById",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> GetTopicById(
        [FromRoute, SwaggerParameter("The ID of the requested topic", Required = true)] int id)
    {
        _log.LogInformation("Searching for topic with ID {TopicId}", id);
        
        var topic = await _topics.GetTopic(id);

        if (topic is not null) return Ok(topic);
        
        _log.LogWarning("Could not locate topic with ID {TopicId}", id);
        
        return NotFound();
    }

    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic name. 
    /// </summary>
    /// <param name="name">The unique name for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided name.</returns>
    [HttpGet("{name}")]
    [SwaggerResponse(StatusCodes.Status200OK, "The requested topic was found.", typeof(Topic))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No topic was found that matches the provided topic name.")]
    [SwaggerOperation(
        Summary = "Get a single topic by name",
        Description = "Returns a single topic matching the provided name.",
        OperationId = "GetTopicByName",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> GetTopicByName(
        [FromRoute, SwaggerParameter("The name of the requested topic", Required = true)] string name)
    {
        _log.LogInformation("Searching for topic with name {TopicName}", name);
        
        var topic = await _topics.GetTopic(name);

        if (topic is not null) return Ok(topic);
        
        _log.LogWarning("Could not locate topic with name {TopicName}", name);

        return NotFound();
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    /// <param name="topic">The new topic to be created.</param>
    /// <returns>The newly created topic, including its updated and current ID.</returns>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "The topic was created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The topic data provided was invalid.")]
    [SwaggerOperation(
        Summary = "Create a new topic",
        Description = "Create a new topic with the provided topic information.",
        OperationId = "CreateTopic",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> CreateTopic(
        [FromBody, SwaggerRequestBody("The values for the new topic to create.", Required = true)] Topic topic)
    {
        _log.LogInformation("Creating new topic with name: {TopicName} description: {TopicDescription}", topic.Name, topic.Description);
        
        Topic newTopic;

        try
        {
            newTopic = await _topics.AddTopic(topic);
            _log.LogInformation("New topic successfully created with ID of {TopicId}", newTopic.Id);
        }
        catch (DuplicateTopicNameException exception)
        {
            _log.LogWarning(exception, "Topic name {TopicName} is already defined", topic.Name);
            return BadRequest($"New name of {topic.Name} is already defined for another topic");
        }

        return CreatedAtAction(nameof(GetTopicById), new {id = newTopic.Id}, newTopic);
    }

    /// <summary>
    /// Update an existing forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for the topic to be updated.</param>
    /// <param name="topic">The topic values to replace the existing topic's values with.</param>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The topic was updated successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The topic data provided was invalid.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No topic was found that matches the provided ID.")]
    [SwaggerOperation(
        Summary = "Update an existing topic",
        Description = "Update an existing topic to match the new provided topic information.",
        OperationId = "UpdateTopic",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> UpdateTopic(
        [FromRoute, SwaggerParameter("The ID of the existing topic to update.", Required = true)] int id,
        [FromBody, SwaggerRequestBody("The topic data to update the existing topic with.", Required = true)] Topic topic
    )
    {
        int numberOfTopicsUpdated;
        
        _log.LogInformation("Updating topic with ID of {TopicId} to id: {NewTopicId} name: {NewTopicName} description: {NewTopicDescription}", id, topic.Id, topic.Name, topic.Description);
        
        try
        {
            numberOfTopicsUpdated = await _topics.UpdateTopic(id, topic);
            _log.LogInformation("Updated {UpdateCount} topics with an ID of {TopicId}", numberOfTopicsUpdated, id);
        }
        catch (NullTopicNameException exception)
        {
            _log.LogWarning(exception, "Topic provided was null");
            return BadRequest("Topic name cannot be null");
        }
        catch (DuplicateIdException exception)
        {
            _log.LogWarning(exception, "New topic ID of {NewTopicId} is already defined for another topic", topic.Id);
            return BadRequest($"New ID of {topic.Id} is already defined for another topic");
        }
        catch (DuplicateTopicNameException exception)
        {
            _log.LogWarning(exception, "New topic name of {NewTopicName} is already defined for another topic", topic.Name);
            return BadRequest($"New name of {topic.Name} is already defined for another topic");
        }

        return numberOfTopicsUpdated == 0 ? NotFound() : NoContent();
    }

    /// <summary>
    /// Delete the topic that matches the ID passed into the URI route.
    /// </summary>
    /// <param name="id">The unique ID for the topic to be deleted.</param>
    [HttpDelete("{id:int}")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The topic was deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No topic was found that matches the provided ID.")]
    [SwaggerOperation(
        Summary = "Delete an existing topic",
        Description = "Delete an existing topic that matches the provided ID.",
        OperationId = "DeleteTopic",
        Tags = new[] { "Topics" }
    )]
    public async Task<IActionResult> DeleteTopic(
        [FromRoute, SwaggerParameter("The ID of the topic to be deleted.", Required = true)] int id)
    {
        _log.LogInformation("Removing topic with ID of {TopicId}", id);
        
        var numberOfTopicsRemoved = await _topics.RemoveTopic(id);
    
        _log.LogInformation("Removed {DeleteCount} topics with an ID of {TopicId}", numberOfTopicsRemoved, id);
        
        return numberOfTopicsRemoved == 0 ? NotFound() : NoContent();
    }

    public class RequestBody
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}