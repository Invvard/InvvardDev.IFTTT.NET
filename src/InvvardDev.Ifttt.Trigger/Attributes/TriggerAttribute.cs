namespace InvvardDev.Ifttt.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : TriggerAttributeBase(slug);