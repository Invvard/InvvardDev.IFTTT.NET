using InvvardDev.Ifttt.Core.Configuration;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Trigger.Controllers;

[ApiController]
[Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController(ITriggerRepository triggerRepository) : ControllerBase
{
    [HttpPost("{triggerSlug}")]
    public IActionResult ExecuteTrigger(string triggerSlug, TriggerRequest triggerRequest)
    {
        var trigger = triggerRepository.GetTriggerProcessorInstance(triggerSlug);

        trigger.ExecuteAsync(triggerRequest);

        return Ok();
    }

    [HttpPost("{triggerSlug}/fields/{fieldName}/validate")]
    public IActionResult ValidateField(string triggerSlug, string fieldName, TriggerFieldValidationRequest triggerRequest)
    {
        var trigger = triggerRepository.GetTriggerProcessorInstance(triggerSlug);

        //var validationResponse = trigger.ValidateField(fieldName, triggerRequest);

        return Ok();
        //return Ok(validationResponse);
    }
}