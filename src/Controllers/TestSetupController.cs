using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Toolkit;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController, Route(IftttConstants.TestingApiPath)]
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

    /// <summary>
    /// Prepares a test setup for the IFTTT service.
    /// </summary>
    /// <returns>A list of test data for IFTTT to use.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> SetupTest(CancellationToken cancellationToken)
    {
        try
        {
            var processors = await testSetup.PrepareSetupListing(cancellationToken);

            var samples = new SamplesPayload(processors);
            samples.SkimEmptyProcessors();

            var payload = new TopLevelMessageModel<SamplesPayload>(samples);

            return Ok(payload);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while setting up test");

            var errorMessages = TopLevelErrorModel.Serialize(new[] { new ErrorMessage($"Error while setting up test: {ex.Message}") });

            return BadRequest(errorMessages);
        }
    }
}
