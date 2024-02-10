namespace InvvardDev.Ifttt.Models.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string slug) : TriggerAttributeBase(slug);