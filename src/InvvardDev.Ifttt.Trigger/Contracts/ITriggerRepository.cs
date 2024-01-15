namespace InvvardDev.Ifttt.Trigger.Contracts;

public interface ITriggerRepository
{
    void AddOrUpdateTrigger(string triggerSlug, Type triggerType);

    void AddOrUpdateTriggerFields(string triggerSlug, Type triggerFieldsType);

    ITrigger GetTriggerProcessorInstance(string triggerSlug);

    Type? GetTriggerFieldsType(string triggerSlug);
}