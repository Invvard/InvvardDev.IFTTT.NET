using InvvardDev.Ifttt.Hosting.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController]
[Route(IftttConstants.TestingApiPath)]
[Consumes("application/json")]
[Produces("application/json")]
public class TestSetupController : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> SetupTest()
    {
        return Task.FromResult<IActionResult>(Ok());
    }

#if DEBUG
    [HttpGet("setup_listing")]
    public Task<IActionResult> GetSetupListing()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
#endif
}