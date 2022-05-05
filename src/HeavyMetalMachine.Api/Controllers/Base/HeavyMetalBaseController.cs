using Microsoft.AspNetCore.Mvc;

namespace HeavyMetalMachine.Api.Controllers.Base;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class HeavyMetalBaseController : ControllerBase { }