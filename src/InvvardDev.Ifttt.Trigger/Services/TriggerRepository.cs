using InvvardDev.Ifttt.Shared.Services;
using InvvardDev.Ifttt.Trigger.Models;

namespace InvvardDev.Ifttt.Trigger.Services;

public class TriggerRepository : ProcessorRepository<TriggerMap>
{
    public override TInterface? GetProcessorInstance<TInterface>(string processorSlug)
        where TInterface : default
    {
        if (Processors.TryGetValue(processorSlug, out var processorMap)
            && Activator.CreateInstance(processorMap.TriggerType) is TInterface processor)
        {
            return processor;
        }

        return default;
    }

    public override void UpsertDataField(string processorSlug, string dataFieldSlug, Type dataFieldType)
    {
        if (GetProcessor(processorSlug) is { } triggerMap)
        {
            triggerMap.TriggerFields.Add(new TriggerField(dataFieldSlug, dataFieldType));
        }
    }

    public override Type? GetDataFieldType(string processorSlug, string dataFieldSlug)
    {
        if (GetProcessor(processorSlug) is { } triggerMap
            && triggerMap.TriggerFields.SingleOrDefault(f => f.TriggerFieldSlug == dataFieldSlug) is { } triggerField)
        {
            return triggerField.TriggerFieldType;
        }
        
        return default;
    }
}