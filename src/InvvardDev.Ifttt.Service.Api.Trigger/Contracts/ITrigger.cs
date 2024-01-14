using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Contracts;

public interface ITrigger
{
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}