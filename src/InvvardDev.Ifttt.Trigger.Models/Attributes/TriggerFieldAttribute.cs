namespace InvvardDev.Ifttt.Trigger.Models.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute(string slug) : TriggerAttributeBase(slug);