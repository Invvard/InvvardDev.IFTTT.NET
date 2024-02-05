using InvvardDev.Ifttt.Shared.Configuration;
using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Models.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Trigger.Controllers;

[ApiController]
[Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController(IProcessorRepository<TriggerMap> triggerRepository) : ControllerBase
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