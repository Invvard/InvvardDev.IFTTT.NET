using System.Reflection;

namespace InvvardDev.Ifttt.Trigger.Models;

internal record TriggerDataType(string TriggerSlug, Type TriggerType)
{
    public Type? TriggerFieldsType { get; init; }

    public Dictionary<string, MethodInfo> TriggerFieldValidator => new();
}