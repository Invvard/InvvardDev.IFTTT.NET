namespace InvvardDev.Ifttt.Toolkit.Attributes;

/// <summary>
/// Attribute to mark a class as a trigger.
/// </summary>
/// <param name="slug">The trigger slug.</param>
[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : ProcessorAttributeBase(slug);