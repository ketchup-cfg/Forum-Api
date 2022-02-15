using Forum.Api.Controllers.Base;
using Forum.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Forum.Data.Models;

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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Topic>> GetAllTopics()
    {
        return await _topics.GetAll();
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
    public async Task<ActionResult<Topic>> GetTopicByName([FromRoute] string name)
    {
        var topic = await _topics.GetTopicByName(name);

        if (topic is null) return NotFound();

        return topic;
    }
}