namespace InvvardDev.Ifttt.Service.Api.Trigger.Triggers;

public interface ITrigger
{
    Task ProcessAsync(CancellationToken cancellationToken = default);
}