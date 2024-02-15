using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController]
[Route(IftttConstants.TestingApiPath)]
[Consumes("application/json")]
[Produces("application/json")]
public class TestSetupController(ITestSetup testSetup) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SetupTest()
    {
        var sample = await testSetup.PrepareSetupListing();

        sample.SkimEmptyProcessors();

        var payload = TopLevelMessageModel<Samples>.Serialize(sample);
        
        return await Task.FromResult<IActionResult>(Ok(payload));
    }
}