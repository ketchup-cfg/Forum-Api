using Microsoft.AspNetCore.Mvc;
using Forum.Data.Models;
using Forum.Data.Queries;

namespace Forum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    [HttpGet]
    public List<Topic> Index()
    {
        return Topics.GetAll();
    }
}