using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Toolkit;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController, Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController([FromKeyedServices(ProcessorKind.Trigger)] IProcessorService triggerService) : ControllerBase
{
    /// <summary>
    /// Execute a trigger 
    /// </summary>
    /// <param name="triggerSlug">The trigger slug that is executed.</param>
    /// <param name="triggerRequest">The request parameters.</param>
    [HttpPost("{triggerSlug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> ExecuteTrigger(string triggerSlug, TriggerRequest triggerRequest)
    {
        if (await triggerService.GetProcessorInstance<ITrigger>(triggerSlug) is not { } trigger)
        {
            return NotFound();
        }

        var result = await trigger.ExecuteAsync(triggerRequest);

        return Ok(result.Serialize());
    }
}
