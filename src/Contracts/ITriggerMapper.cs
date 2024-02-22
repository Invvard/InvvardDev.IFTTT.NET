namespace InvvardDev.Ifttt.Contracts;

public interface ITriggerMapper
{
    Task MapTriggerProcessors(CancellationToken stoppingToken);

    Task MapTriggerFields(CancellationToken stoppingToken);
}