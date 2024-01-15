using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.Repositories;

internal class TriggerRepositoryService : ITriggerRepository
{
    private readonly Dictionary<string, TriggerDataType> triggers = new();

    public void AddOrUpdateTrigger(string triggerSlug, Type triggerType)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerData))
        {
            triggers.Add(triggerSlug, new TriggerDataType(triggerSlug, triggerType));
        }
        else
        {
            triggers[triggerSlug] = triggerData with
                                    {
                                        TriggerType = triggerType
                                    };
        }
    }

    public void AddOrUpdateTriggerFields(string triggerSlug, Type triggerFieldsType)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerData))
        {
            throw new InvalidOperationException($"Trigger '{triggerSlug}' was not found.");
        }

        triggers[triggerSlug] = triggerData with
                                {
                                    TriggerFieldsType = triggerFieldsType
                                };
    }

    public ITrigger GetTriggerProcessorInstance(string triggerSlug)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerDataType)
            || Activator.CreateInstance(triggerDataType.TriggerType) is not ITrigger triggerInstance)
        {
            throw new InvalidOperationException($"Trigger '{triggerSlug}' was not found.");
        }

        return triggerInstance;
    }

    public Type? GetTriggerFieldsType(string triggerSlug)
    {
        triggers.TryGetValue(triggerSlug, out var triggerDataType);

        return triggerDataType?.TriggerFieldsType;
    }
}