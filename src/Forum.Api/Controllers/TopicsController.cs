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
    /// <param name="id">The unique ID for a single topic.</param>
    /// <param name="topic">The topic values to replace the existing topic's values with.</param>
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
            return BadRequest("Topic name is not unique and is already defined for another topic");
        }

        return numberOfTopicsUpdated == 0 ? NotFound() : NoContent();
    }
}