using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Toolkit.Contracts;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

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