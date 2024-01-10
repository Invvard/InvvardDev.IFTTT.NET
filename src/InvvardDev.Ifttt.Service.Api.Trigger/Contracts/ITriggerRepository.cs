namespace InvvardDev.Ifttt.Service.Api.Trigger.Contracts;

public interface ITriggerRepository
{
    ITriggerRepository MapTriggerTypes();
    
    ITriggerRepository MapTriggerFields();
    
    ITrigger GetTriggerProcessorInstance(string triggerSlug);
    
    Type? GetTriggerFieldsType(string triggerSlug);
}