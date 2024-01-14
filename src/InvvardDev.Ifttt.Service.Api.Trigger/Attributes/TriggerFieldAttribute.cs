namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute(string slug) : TriggerAttributeBase(slug);