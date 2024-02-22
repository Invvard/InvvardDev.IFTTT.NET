using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController]
[Route(IftttConstants.TestingApiPath)]
[Consumes("application/json")]
[Produces("application/json")]
public class TestSetupController : ControllerBase
{
    private readonly ITestSetup testSetup;
    private readonly ILogger<TestSetupController> logger;

    public TestSetupController(ITestSetup testSetup, ILogger<TestSetupController> logger)
    {
        ArgumentNullException.ThrowIfNull(testSetup);
        this.testSetup = testSetup;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SetupTest()
    {
        try
        {
            var processors = await testSetup.PrepareSetupListing();

            var samples = new SamplesPayload(processors);
            samples.SkimEmptyProcessors();

            var payload = new TopLevelMessageModel<SamplesPayload>(samples);
            
            return Ok(payload);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while setting up test");

            var errorMessages = TopLevelErrorModel.Serialize(new[] { new ErrorMessage($"Error while setting up test: {ex.Message}") });

            return Problem(errorMessages);
        }
    }
}
