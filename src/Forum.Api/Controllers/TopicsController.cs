using Forum.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Forum.Data.Models;

namespace Forum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ITopics _topics;
    
    public TopicsController(ITopics topics)
    {
        _topics = topics;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Topic>> Index()
    {
        return await _topics.GetAll();
    }
}