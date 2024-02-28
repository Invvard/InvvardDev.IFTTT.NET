namespace InvvardDev.Ifttt.Toolkit.Attributes;

/// <summary>
/// Attribute to mark a class holding trigger fields for a trigger.
/// </summary>
/// <param name="slug">The trigger slug whose fields are related to.</param>
[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : ProcessorAttributeBase(slug);