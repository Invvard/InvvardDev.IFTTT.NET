namespace InvvardDev.Ifttt.Service.Api.Trigger.Contracts;

public interface ITriggerRepository
{
    void MapTriggerTypes();
    ITrigger GetTriggerProcessorInstance(string triggerSlug);
}