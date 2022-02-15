using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ForumBaseController : ControllerBase { }