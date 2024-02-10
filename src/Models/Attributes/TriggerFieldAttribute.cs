namespace InvvardDev.Ifttt.Models.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute(string slug) : TriggerAttributeBase(slug);