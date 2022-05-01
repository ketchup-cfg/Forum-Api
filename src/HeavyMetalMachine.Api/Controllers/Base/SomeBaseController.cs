using Microsoft.AspNetCore.Mvc;

namespace WallOfNoise.Api.Controllers.Base;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class SomeBaseController : ControllerBase
{
}