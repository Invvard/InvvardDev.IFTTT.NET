using InvvardDev.Ifttt.Shared.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Core.Controllers;

[ApiController]
[Route(IftttConstants.StatusApiPath)]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok();
    }
}