using InvvardDev.Ifttt.Core.Configuration;
using InvvardDev.Ifttt.Core.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Trigger.Controllers;

[ApiController]
[Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController(IServiceRepository triggerRepository) : ControllerBase
{
    [HttpPost("{triggerSlug}")]
    public IActionResult ExecuteTrigger(string triggerSlug, TriggerRequest triggerRequest)
    {
        if (triggerRepository.GetProcessorInstance<ITrigger>(triggerSlug) is not { } trigger)
        {
            return NotFound();
        }

        trigger.ExecuteAsync(triggerRequest);
            
        return Ok();
    }
}