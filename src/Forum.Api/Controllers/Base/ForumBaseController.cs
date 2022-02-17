using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableCors]
public abstract class ForumBaseController : ControllerBase { }