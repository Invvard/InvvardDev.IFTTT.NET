using InvvardDev.Ifttt.Service.Api.Core.Configuration;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;
using InvvardDev.Ifttt.Service.Api.Trigger.TriggerRepository;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Controllers;

[ApiController]
[Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController(ITriggerRepository triggerRepository) : ControllerBase
{
    [HttpPost("{triggerSlug}")]
    public IActionResult ExecuteTrigger(string triggerSlug, TriggerRequestBase triggerRequest)
    {
        var trigger = triggerRepository.GetTriggerProcessorInstance(triggerSlug);

        trigger.ExecuteAsync(triggerRequest);
            
        return Ok();
    }
}