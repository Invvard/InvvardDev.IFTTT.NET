using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Service.Api.Core.Controllers;

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