using Forum.Api.Controllers.Attributes;
using Forum.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Forum.Data.Models;
using Forum.Services.Abstractions;

namespace Forum.Api.Controllers;

public class TopicsController : ForumBaseController
{
    private readonly ITopicsService _topics;
    
    public TopicsController(ITopicsService topics)
    {
        _topics = topics;
    }
    
    /// <summary>
    /// Find and return a collection of all existing forum topics.
    /// </summary>
    /// <returns>A collection containing data for all existing forum topics.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Topic>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTopics()
    {
        var topics = await _topics.GetAllTopics();

        return Ok(topics);
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided ID.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Topic), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopicById([FromRoute] int id)
    {
        var topic = await _topics.GetTopic(id);

        if (topic is null) return NotFound();

        return Ok(topic);
    }
    
    /// <summary>
    /// Find and return information for a single forum topic matching the specified topic name. 
    /// </summary>
    /// <param name="name">The unique name for a single topic.</param>
    /// <returns>The data for the forum topic that was found using the provided name.</returns>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(Topic), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTopicByName([FromRoute] string name)
    {
        var topic = await _topics.GetTopic(name);

        if (topic is null) return NotFound();

        return Ok(topic);
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    /// <param name="topic">The new topic to be created.</param>
    /// <returns>The newly created topic, including its updated and current ID.</returns>
    [HttpPost]
    [EnsureTopicNameIsUnique]
    [ProducesResponseType(typeof(Topic), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTopic([FromBody] Topic topic)
    {
        var newTopic = await _topics.AddTopic(topic);

        return CreatedAtAction(nameof(GetTopicById), new {id = newTopic.Id}, newTopic);
    }

    /// <summary>
    /// Update an existing forum topic matching the specified topic ID. 
    /// </summary>
    /// <param name="id">The unique ID for the topic to be updated.</param>
    /// <param name="topic">The topic values to replace the existing topic's values with.</param>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [EnsureNewTopicIdIsUnique]
    [EnsureNewTopicNameIsUnique]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTopic(
        [FromRoute] int id,
        [FromBody] Topic topic
        )
    {
        if (!await _topics.TopicExists(id))
        {
            return NotFound();
        }
        
        await _topics.UpdateTopic(id, topic);

        return NoContent();
    }

    /// <summary>
    /// Delete the topic that matches the ID passed into the URI route.
    /// </summary>
    /// <param name="id">The unique ID for the topic to be deleted.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTopic([FromRoute] int id)
    {
        if (!await _topics.TopicExists(id))
        {
            return NotFound();
        }
        
        await _topics.RemoveTopic(id);

        return NoContent();
    }
}