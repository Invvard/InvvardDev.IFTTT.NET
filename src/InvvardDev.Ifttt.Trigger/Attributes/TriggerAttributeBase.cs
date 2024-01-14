namespace InvvardDev.Ifttt.Trigger.Attributes;

public class TriggerAttributeBase : Attribute
{
    public string Slug { get; }
    
    protected TriggerAttributeBase(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Trigger slug cannot be null or whitespace.", nameof(slug));
        }
        
        Slug = slug;
    }
}