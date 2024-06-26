using System.Reflection;
using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Models.Core;
using InvvardDev.Ifttt.Models.Trigger;
using InvvardDev.Ifttt.Toolkit.Attributes;

namespace InvvardDev.Ifttt.Reflection;

internal class TriggerMapper(
    [FromKeyedServices(ProcessorKind.Trigger)] IProcessorService triggerService,
    [FromKeyedServices(nameof(TriggerAttributeLookup))] IAttributeLookup triggerAttributeLookup,
    [FromKeyedServices(nameof(TriggerFieldsAttributeLookup))] IAttributeLookup triggerFieldsAttributeLookup,
    ILogger<TriggerMapper> logger) : ITriggerMapper
{
    public async Task MapTriggerProcessors(CancellationToken stoppingToken)
        => await MapAttribute<TriggerAttribute>(triggerAttributeLookup.GetAnnotatedTypes(),
                                                async (triggerSlug, type) => await triggerService.GetProcessor(triggerSlug) switch
                                                                             {
                                                                                 { } existingProcessorTree when existingProcessorTree.ProcessorType == type
                                                                                     => true,
                                                                                 { } existingProcessorTree when existingProcessorTree.ProcessorType != type
                                                                                     => throw new
                                                                                         InvalidOperationException($"Conflict: 'Trigger' processor with slug '{triggerSlug}' already exists (Type is '{existingProcessorTree.ProcessorType}')."),
                                                                                 _ => false,
                                                                             },
                                                stoppingToken);

    public async Task MapTriggerFields(CancellationToken stoppingToken)
        => await MapAttribute<TriggerFieldsAttribute>(triggerFieldsAttributeLookup.GetAnnotatedTypes(),
                                                      async (triggerSlug, _) => await triggerService.Exists(triggerSlug) switch
                                                                                {
                                                                                    true => true,
                                                                                    false => throw new InvalidOperationException($"There is no trigger with slug '{triggerSlug}' registered.")
                                                                                },
                                                      stoppingToken);

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

    private async Task<TriggerMapper> MapAttribute<TAttribute>(IEnumerable<Type> types, Func<string, Type, Task<bool>> processorExists, CancellationToken stoppingToken = default)
        where TAttribute : ProcessorAttributeBase
    {
        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            foreach (var type in types)
            {
                if (type.GetCustomAttribute<TAttribute>() is not { } attribute) continue;

                if (!await processorExists(attribute.Slug, type))
                {
                    await triggerService.AddOrUpdateProcessor(new ProcessorTree(attribute.Slug, type, ProcessorKind.Trigger));
                }

                await MapTriggerFieldProperties(attribute.Slug, type);
            }
        }
        catch (OperationCanceledException ocex)
        {
            logger.LogInformation(ocex, "Mapping for attribute '{AttributeType}' was cancelled.", typeof(TAttribute));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Mapping for attribute '{AttributeType}' has failed.", typeof(TAttribute));
        }

        return this;
    }
}
