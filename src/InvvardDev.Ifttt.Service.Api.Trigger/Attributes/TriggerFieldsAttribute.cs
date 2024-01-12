namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : TriggerAttributeBase (slug);