using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Reflection;

internal class TriggerMapper(
    [FromKeyedServices(ProcessorKind.Trigger)] IProcessorService triggerService,
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup) : ITriggerMapper
{
    public async Task<ITriggerMapper> MapTriggerProcessors()
        => await MapAttribute<TriggerAttribute>(triggerAttributeLookup.GetAnnotatedTypes(),
                                                async (attribute, type) => await triggerService.GetProcessor(attribute.Slug) switch
                                                                           {
                                                                               { } triggerMap when triggerMap.Type == type => triggerMap,
                                                                               not null => throw new InvalidOperationException("Trigger has already been registered"),
                                                                               _ => new ProcessorTree(attribute.Slug, type, ProcessorKind.Trigger)
                                                                           });

    public async Task<ITriggerMapper> MapTriggerFields()
        => await MapAttribute<TriggerFieldsAttribute>(triggerFieldsAttributeLookup.GetAnnotatedTypes(),
                                                      (attribute, _) => triggerService.GetProcessor(attribute.Slug));

    private async Task MapTriggerFieldProperties(string parentSlug, Type parentType)
    {
        var properties = parentType.GetProperties();
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<DataFieldAttribute>() is { } triggerFieldAttribute)
            {
                await triggerService.AddDataField(parentSlug, triggerFieldAttribute.Slug, property.PropertyType);
            }
        }
    }

    private async Task<TriggerMapper> MapAttribute<TAttribute>(IEnumerable<Type> types, Func<TAttribute, Type, Task<ProcessorTree?>> getProcessor)
        where TAttribute : ProcessorAttributeBase
    {
        foreach (var type in types)
        {
            if (type.GetCustomAttribute<TAttribute>() is not { } attribute
                || await getProcessor(attribute, type) is not { } triggerTree) continue;

            await triggerService.AddOrUpdateProcessor(triggerTree);
            await MapTriggerFieldProperties(attribute.Slug, type);
        }

        return this;
    }
}