namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute : Attribute
{
    public TriggerAttribute(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Trigger slug cannot be null or whitespace.", nameof(slug));
        }
        
        Slug = slug;
    }

    public string Slug { get; }
}