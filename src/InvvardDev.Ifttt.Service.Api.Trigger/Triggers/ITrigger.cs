using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Triggers;

public interface ITrigger
{   
    Task ExecuteAsync(TriggerRequestBase triggerRequest, CancellationToken cancellationToken = default);
}