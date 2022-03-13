using Microsoft.AspNetCore.Mvc;

namespace Somewhere.Api.Controllers;

[ApiController]
public class ErrorController : Controller
{

    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        return Problem();
    }
}