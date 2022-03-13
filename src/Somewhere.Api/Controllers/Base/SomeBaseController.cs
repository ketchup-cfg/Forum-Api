using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Somewhere.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableCors]
public abstract class SomeBaseController : ControllerBase { }