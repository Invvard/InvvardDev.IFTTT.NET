namespace InvvardDev.Ifttt.Toolkit.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : TriggerAttributeBase(slug);