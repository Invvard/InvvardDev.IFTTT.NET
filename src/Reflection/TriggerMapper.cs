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
                                                async (triggerSlug, type) => await triggerService.GetProcessor(triggerSlug) switch
                                                                             {
                                                                                 null => false,
                                                                                 { } existingProcessorTree when existingProcessorTree.Type == type
                                                                                     => true,
                                                                                 { } pt when pt.Type != type
                                                                                     => throw new InvalidOperationException($"Conflict: 'Trigger' processor with slug '{triggerSlug}' already exists (Type is '{pt.Type}')."),
                                                                                 _ => throw new ArgumentOutOfRangeException()
                                                                             });

    public async Task<ITriggerMapper> MapTriggerFields()
        => await MapAttribute<TriggerFieldsAttribute>(triggerFieldsAttributeLookup.GetAnnotatedTypes(),
                                                      async (triggerSlug, _) => await triggerService.Exists(triggerSlug) switch
                                                                                {
                                                                                    true => true,
                                                                                    false => throw new InvalidOperationException($"There is no trigger with slug '{triggerSlug}' registered.")
                                                                                });

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

    private async Task<TriggerMapper> MapAttribute<TAttribute>(IEnumerable<Type> types, Func<string, Type, Task<bool>> processorExists)
        where TAttribute : ProcessorAttributeBase
    {
        foreach (var type in types)
        {
            if (type.GetCustomAttribute<TAttribute>() is not { } attribute) continue;

            if (await processorExists(attribute.Slug, type) is false)
            {
                await triggerService.AddOrUpdateProcessor(new ProcessorTree(attribute.Slug, type, ProcessorKind.Trigger));
            }

            await MapTriggerFieldProperties(attribute.Slug, type);
        }

        return this;
    }
}