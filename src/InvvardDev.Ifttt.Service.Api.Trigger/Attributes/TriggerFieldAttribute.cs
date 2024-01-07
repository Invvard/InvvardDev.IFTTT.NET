namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TriggerFieldAttribute : Attribute
{
    public string Slug { get; set; }

    public TriggerFieldAttribute(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Trigger field slug cannot be null or whitespace.", nameof(slug));
        }

        Slug = slug;
    }
}