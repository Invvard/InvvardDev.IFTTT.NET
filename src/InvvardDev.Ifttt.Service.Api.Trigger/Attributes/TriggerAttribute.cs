namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : TriggerAttributeBase(slug);