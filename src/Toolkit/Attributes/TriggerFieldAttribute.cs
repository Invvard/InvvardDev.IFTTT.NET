namespace InvvardDev.Ifttt.Toolkit.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute(string slug) : TriggerAttributeBase(slug);