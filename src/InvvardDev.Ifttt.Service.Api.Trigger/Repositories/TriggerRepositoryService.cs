using System.Reflection;
using InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Repositories;

internal class TriggerRepositoryService(
    [FromKeyedServices(nameof(TriggerAttributeLookup))]
    IAttributeLookup triggerAttributeLookup) : ITriggerRepository
{
    private readonly Dictionary<string, ITrigger> triggers = new();

    public void MapTriggerTypes()
    {
        var types = triggerAttributeLookup.GetAnnotatedTypes();
        foreach (var triggerType in types)
        {
            if (triggerType.GetCustomAttribute<TriggerAttribute>() is { } triggerAttribute
                && !triggers.ContainsKey(triggerAttribute.Slug))
            {
                var triggerInstance = Activator.CreateInstance(triggerType) as ITrigger
                                      ?? throw new InvalidOperationException();
                triggers.Add(triggerAttribute.Slug, triggerInstance);
            }
        }
    }

    public ITrigger GetTriggerProcessorInstance(string triggerSlug)
    {
        if (!triggers.TryGetValue(triggerSlug, out var triggerInstance))
        {
            throw new InvalidOperationException();
        }

        return triggerInstance;
    }
}