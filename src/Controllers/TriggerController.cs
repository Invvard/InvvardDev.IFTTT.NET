using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Hosting.Models;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Toolkit.Contracts;
using InvvardDev.Ifttt.Toolkit.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvvardDev.Ifttt.Controllers;

[ApiController]
[Route(IftttConstants.BaseTriggersApiPath)]
public class TriggerController([FromKeyedServices(ProcessorKind.Trigger)] IProcessorService triggerService) : ControllerBase
{
    [HttpPost("{triggerSlug}")]
    public async Task<IActionResult> ExecuteTrigger(string triggerSlug, TriggerRequest triggerRequest)
    {
        if (await triggerService.GetProcessorInstance<ITrigger>(triggerSlug) is not { } trigger)
        {
            return NotFound();
        }

        await trigger.ExecuteAsync(triggerRequest);

        return Ok();
    }
}