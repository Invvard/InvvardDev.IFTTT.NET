namespace InvvardDev.Ifttt.Trigger.Models.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : TriggerAttributeBase(slug);