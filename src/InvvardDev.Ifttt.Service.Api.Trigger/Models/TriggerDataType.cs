namespace InvvardDev.Ifttt.Service.Api.Trigger.Models;

internal record TriggerDataType(string TriggerSlug, Type TriggerType)
{
    public Type? TriggerFieldsType { get; init; }
}