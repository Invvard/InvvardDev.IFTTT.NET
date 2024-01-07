using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Service.Api.Core.Controllers;

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
}