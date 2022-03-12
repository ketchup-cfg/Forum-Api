using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers;

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