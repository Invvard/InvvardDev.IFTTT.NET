using System.Reflection;
using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Models.Attributes;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal class TriggerMapper(
    IProcessorRepository<TriggerMap> triggerRepository,
    IAssemblyAccessor assemblyAccessor,
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup) : ITriggerMapper
{
    public ITriggerMapper MapTriggerProcessors()
    {
        var types = triggerAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerType in types)
        {
            if (assemblyAccessor.GetAttribute<TriggerAttribute>(triggerType) is { } triggerAttribute)
            {
                triggerRepository.UpsertProcessor(triggerAttribute.Slug, triggerType);
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
                triggerRepository.UpsertType(triggerFieldsAttribute.Slug, triggerFieldsType);
            }
        }

        return this;
    }
}