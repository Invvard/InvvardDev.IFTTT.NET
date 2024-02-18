namespace InvvardDev.Ifttt.Toolkit.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : ProcessorAttributeBase(slug);