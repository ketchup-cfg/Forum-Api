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
    /// <returns>A collection containing data for all existing forum topics, if any exist.</returns>
    /// <response code="200">Topics are defined and were able to be located.</response>
    /// <response code="204">No topics are currently defined.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType((StatusCodes.Status204NoContent))]
    public async Task<IActionResult> GetAllTopics()
    {
        var topics = await _topics.GetAll();
        
        if (topics.Any())
        {
            return Ok(topics);
        }
        else
        {
            return NoContent();
        }
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided ID.</returns>
    /// <response code="200">The requested topic was found.</response>
    /// <response code="404">No topic was found matching the ID passed into the URI path.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Topic>> GetTopicById([FromRoute] int id)
    {
        var topic = await _topics.GetTopicById(id);

        if (topic is null) return NotFound();

        return topic;
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic name. 
    /// </summary>
    /// <param name="name">The unique name for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided name.</returns>
    /// <response code="200">The requested topic was found.</response>
    /// <response code="404">No topic was found matching the name passed into the URI path.</response>
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Topic>> GetTopicByName([FromRoute] string name)
    {
        var topic = await _topics.GetTopicByName(name);

        if (topic is null) return NotFound();

        return topic;
    }

    /// <summary>
    /// Update an existing forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single topic.</param>
    /// <param name="topic">The topic values to replace the existing topic's values with.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/topics/1
    ///     {
    ///        "id": 1,
    ///        "name": "Weather",
    ///        "Description": "Some weather we've been having, eh?"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">The update was successful.</response>
    /// <response code="400">The name is null, an empty string, or a different topic already has this name.</response>
    /// <response code="404">No topic was found matching the ID passed into the URI path.</response>
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
            numberOfTopicsUpdated = await _topics.UpdateTopic(topic);
        }
        catch (PostgresException e) when (e.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return BadRequest("Name of topic is invalid");
        }

        return numberOfTopicsUpdated == 0 ? NotFound() : NoContent();
    }
}