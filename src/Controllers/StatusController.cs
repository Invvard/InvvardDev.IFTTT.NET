using InvvardDev.Ifttt.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController]
[Route(IftttConstants.StatusApiPath)]
public class StatusController : ControllerBase
{
    /// <summary>
    /// Get the status of the service.
    /// </summary>
    /// <returns>HTTP Code 200 OK if service is up.</returns>
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok();
    }
}