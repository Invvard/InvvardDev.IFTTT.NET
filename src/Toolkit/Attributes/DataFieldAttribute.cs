namespace InvvardDev.Ifttt.Toolkit.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DataFieldAttribute(string slug) : ProcessorAttributeBase(slug);