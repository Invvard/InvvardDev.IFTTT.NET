namespace InvvardDev.Ifttt.Toolkit.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : ProcessorAttributeBase(slug);