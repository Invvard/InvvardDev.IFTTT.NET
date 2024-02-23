namespace InvvardDev.Ifttt.Toolkit.Attributes;

/// <summary>
/// Attribute to mark a property as a data field.
/// </summary>
/// <param name="slug">The data field slug</param>
[AttributeUsage(AttributeTargets.Property)]
public class DataFieldAttribute(string slug) : ProcessorAttributeBase(slug);