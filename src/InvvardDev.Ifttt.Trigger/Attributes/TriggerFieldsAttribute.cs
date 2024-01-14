namespace InvvardDev.Ifttt.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : TriggerAttributeBase (slug);