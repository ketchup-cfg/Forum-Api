using System.Diagnostics;
using Forum.Api.Controllers.Base;
using Forum.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Forum.Data.Models;
using Npgsql;

namespace Forum.Api.Controllers;

public class TopicsController : ForumBaseController
{
    private readonly ITopics _topics;
    
    public TopicsController(ITopics topics)
    {
        _topics = topics;
    }
    
    /// <summary>
    /// Find and return a collection of all existing forum topics.
    /// </summary>
    /// <returns>A collection containing data for all existing forum topics.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/topics
    ///
    /// </remarks>
    /// <response code="200">Returns the collection of all existing forum topics.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTopics()
    {
        var topics = await _topics.GetAll();

        return Ok(topics);
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided ID.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/topics/1
    ///
    /// </remarks>
    /// <response code="200">Returns the found topic.</response>
    /// <response code="404">No topic was found that matches the provided ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopicById([FromRoute] int id)
    {
        var topic = await _topics.GetTopicById(id);

        if (topic is null) return NotFound();

        return Ok(topic);
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic name. 
    /// </summary>
    /// <param name="name">The unique name for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided name.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/topics/weather
    ///
    /// </remarks>
    /// <response code="200">Returns the found topic.</response>
    /// <response code="404">No topic was found that matches the provided name.</response>
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopicByName([FromRoute] string name)
    {
        var topic = await _topics.GetTopicByName(name);

        if (topic is null) return NotFound();

        return Ok(topic);
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    /// <param name="topic">The new topic to be created.</param>
    /// <returns>The newly created topic, including its updated and current ID.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/topics
    ///     {
    ///         "name": "weather",
    ///         "description": "Let's talk about the weather."
    ///     }
    ///
    /// </remarks>
    /// <response code="201">
    /// Returns the newly created topic, and the URI for this topic in the "location" header.
    /// </response>
    /// <response code="400">
    /// There is an issue with the data that was sent for the new topic, please see the response body for more
    /// information
    /// </response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTopic([FromBody] Topic topic)
    {
        try
        {
            topic.Id = await _topics.CreateTopic(topic);
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return BadRequest("Topic name is not unique and is already defined for another topic");
        }

        return CreatedAtAction(nameof(GetTopicById), new {id = topic.Id}, topic);
    }

    /// <summary>
    /// Update an existing forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for the topic to be updated.</param>
    /// <param name="topic">The topic values to replace the existing topic's values with.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/topics/1
    ///     {
    ///         "id": 1,
    ///         "name": "weathered",
    ///         "description": "Let's talk about the weathered rocks."
    ///     }
    ///
    /// </remarks>
    /// <response code="204">
    /// The topic was successfully updated with no issues.
    /// </response>
    /// <response code="400">
    /// There is an issue with the data that was sent for the topic, please see the response body for more information.
    /// </response>
    /// <response code="404">
    /// No topic was found that matches the ID passed into the URI route.
    /// </response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTopic(
        [FromRoute] int id,
        [FromBody] Topic topic
        )
    {
        var numberOfTopicsUpdated = 0;

        try
        {
            numberOfTopicsUpdated = await _topics.UpdateTopic(id, topic);
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation &&
                                          e.ConstraintName == "topics_pkey")
        {
            return BadRequest("New ID is not valid and is already defined for another topic and cannot be changed.");
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation &&
                                          e.ConstraintName == "topics_name_key")
        {
            return BadRequest("New name is not valid and is already defined for another topic and cannot be changed.");
        }

        return numberOfTopicsUpdated == 0 ? NotFound() : NoContent();
    }

    /// <summary>
    /// Delete the topic that matches the ID passed into the URI route.
    /// </summary>
    /// <param name="id">The unique ID for the topic to be deleted.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/topics/1
    ///
    /// </remarks>
    /// <response code="204">The topic was successfully deleted with no issues.</response>
    /// <response code="404">No topic was found that matches the ID passed into the URI route.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTopic([FromRoute] int id)
    {
        var numberOfTopicsDeleted = await _topics.DeleteTopic(id);

        return numberOfTopicsDeleted == 0 ? NotFound() : NoContent();
    }
}