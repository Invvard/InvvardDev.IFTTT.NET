using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;
using InvvardDev.Ifttt.Service.Api.Trigger.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Repositories;

internal class TriggerRepositoryService(
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup
) : ITriggerRepository
{
    private readonly Dictionary<string, TriggerDataType> triggers = new();

    public ITriggerRepository MapTriggerTypes()
    {
        var types = triggerAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerType in types)
        {
            if (triggerType.GetCustomAttribute<TriggerAttribute>() is { } triggerAttribute
                && !triggers.ContainsKey(triggerAttribute.Slug))
            {
                triggers.Add(triggerAttribute.Slug, new TriggerDataType(triggerAttribute.Slug, triggerType));
            }
        }

        return this;
    }

    public ITriggerRepository MapTriggerFields()
    {
        var types = triggerFieldsAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerFieldsType in types)
        {
            if (triggerFieldsType.GetCustomAttribute<TriggerFieldsAttribute>() is { } triggerFieldsAttribute
                && triggers.TryGetValue(triggerFieldsAttribute.Slug, out var triggerDataType))
            {
                triggers[triggerFieldsAttribute.Slug] = triggerDataType with
                                                               {
                                                                   TriggerFieldsType = triggerFieldsType
                                                               };
            }
        }

        return this;
    }

    public ITrigger GetTriggerProcessorInstance(string triggerSlug)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerDataType)
            || Activator.CreateInstance(triggerDataType.TriggerType) is not ITrigger triggerInstance)
        {
            throw new InvalidOperationException();
        }

        return triggerInstance;
    }

    public Type? GetTriggerFieldsType(string triggerSlug)
    {
        triggers.TryGetValue(triggerSlug, out var triggerDataType);

        return triggerDataType?.TriggerFieldsType;
    }
}