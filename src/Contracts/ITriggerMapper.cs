namespace InvvardDev.Ifttt.Contracts;

public interface ITriggerMapper
{
    Task<ITriggerMapper> MapTriggerProcessors();

    Task<ITriggerMapper> MapTriggerFields();
}