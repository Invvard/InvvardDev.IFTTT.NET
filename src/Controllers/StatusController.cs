using InvvardDev.Ifttt.Hosting.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

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