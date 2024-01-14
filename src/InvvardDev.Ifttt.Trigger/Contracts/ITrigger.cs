using InvvardDev.Ifttt.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface ITrigger
{
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}