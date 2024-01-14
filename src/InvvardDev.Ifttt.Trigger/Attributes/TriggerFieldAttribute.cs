namespace InvvardDev.Ifttt.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute(string slug) : TriggerAttributeBase(slug);