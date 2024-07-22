using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Toolkit;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController, Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController([FromKeyedServices(ProcessorKind.Trigger)] IProcessorService triggerService, ILogger<TriggerController> logger) : ControllerBase
{
    /// <summary>
    /// Execute a trigger 
    /// </summary>
    /// <param name="triggerSlug">The trigger slug that is executed.</param>
    /// <param name="triggerRequest">The request parameters.</param>
    [HttpPost("{triggerSlug}")]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json"), Produces("application/json")]
    public async Task<IActionResult> ExecuteTrigger(string triggerSlug, TriggerRequest triggerRequest)
    {
        if (await triggerService.GetProcessorInstance<ITrigger>(triggerSlug) is not { } trigger)
        {
            return NotFound();
        }

        if (await trigger.ExecuteAsync(triggerRequest) is not { } result)
        {
            throw new InvalidOperationException("The trigger response is invalid: the response is null.");
        }

        if (result.Count > triggerRequest.Limit)
        {
            logger.LogWarning("The trigger response data count ({DataCount}) is greater than the limit ({Limit}). Excess data will be removed ", result.Count, triggerRequest.Limit);
            result = result.Take(triggerRequest.Limit).ToList();
        }

        return Ok(result);
    }
}
