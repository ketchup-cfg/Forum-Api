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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Topic>> Index()
    {
        return await _topics.GetAll();
    }
}