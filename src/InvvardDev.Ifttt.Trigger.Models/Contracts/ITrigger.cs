namespace InvvardDev.Ifttt.Trigger.Models.Contracts;

public interface ITrigger
{
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}