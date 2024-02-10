using InvvardDev.Ifttt.Models.Trigger;

namespace InvvardDev.Ifttt.Contracts;

public interface ITrigger
{
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}