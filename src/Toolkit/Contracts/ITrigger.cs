using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Toolkit.Contracts;

public interface ITrigger
{
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}