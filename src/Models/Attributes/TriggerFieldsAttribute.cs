namespace InvvardDev.Ifttt.Models.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerFieldsAttribute(string slug) : TriggerAttributeBase(slug);