using System.Reflection;
using InvvardDev.Ifttt.Trigger.Attributes;
using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal class TriggerMapper(
    ITriggerRepository triggerRepository,
    IAssemblyAccessor assemblyAccessor,
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup) : ITriggerMapper
{
    public ITriggerMapper MapTriggerTypes()
    {
        var types = triggerAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerType in types)
        {
            if (assemblyAccessor.GetAttribute<TriggerAttribute>(triggerType) is { } triggerAttribute)
            {
                triggerRepository.AddOrUpdateTrigger(triggerAttribute.Slug, triggerType);
            }
        }

        return this;
    }

    public ITriggerMapper MapTriggerFields()
    {
        var types = triggerFieldsAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerFieldsType in types)
        {
            if (triggerFieldsType.GetCustomAttribute<TriggerFieldsAttribute>() is { } triggerFieldsAttribute)
            {
                triggerRepository.AddOrUpdateTriggerFields(triggerFieldsAttribute.Slug, triggerFieldsType);
            }
        }

        return this;
    }
}