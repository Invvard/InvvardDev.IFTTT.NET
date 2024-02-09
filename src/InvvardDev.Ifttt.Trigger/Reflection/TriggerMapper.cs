using System.Reflection;
using InvvardDev.Ifttt.Shared.Contracts;
using InvvardDev.Ifttt.Trigger.Contracts;
using InvvardDev.Ifttt.Trigger.Models;
using InvvardDev.Ifttt.Trigger.Models.Attributes;

namespace InvvardDev.Ifttt.Trigger.Reflection;

internal class TriggerMapper(
    IProcessorRepository<TriggerMap> triggerRepository,
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup) : ITriggerMapper
{
    public ITriggerMapper MapTriggerProcessors()
        => MapAttribute<TriggerAttribute>(triggerAttributeLookup.GetAnnotatedTypes(),
                                          (attribute, type) => triggerRepository.GetProcessor(attribute.Slug) switch
                                                               {
                                                                   { } triggerMap when triggerMap.TriggerType == type => triggerMap,
                                                                   not null => throw new InvalidOperationException("Trigger has already been registered"),
                                                                   _ => new TriggerMap(attribute.Slug, type)
                                                               });

    public ITriggerMapper MapTriggerFields()
        => MapAttribute<TriggerFieldsAttribute>(triggerFieldsAttributeLookup.GetAnnotatedTypes(),
                                                (attribute, _) => triggerRepository.GetProcessor(attribute.Slug));

    private static List<TriggerField> MapTriggerFieldProperties(Type parentType)
    {
        var properties = parentType.GetProperties();
        var triggerFields = new List<TriggerField>();
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<TriggerFieldAttribute>() is { } triggerFieldAttribute)
            {
                triggerFields.Add(new TriggerField(triggerFieldAttribute.Slug, property.PropertyType));
            }
        }

        return triggerFields;
    }

    private TriggerMapper MapAttribute<TAttribute>(IEnumerable<Type> types, Func<TAttribute, Type, TriggerMap?> getProcessor)
        where TAttribute : Attribute
    {
        foreach (var type in types)
        {
            if (type.GetCustomAttribute<TAttribute>() is not { } attribute
                || getProcessor(attribute, type) is not { } triggerMap) continue;

            triggerMap.TriggerFields.AddRange(MapTriggerFieldProperties(type));
            triggerRepository.UpsertProcessor(triggerMap.TriggerSlug, triggerMap);
        }

        return this;
    }
}