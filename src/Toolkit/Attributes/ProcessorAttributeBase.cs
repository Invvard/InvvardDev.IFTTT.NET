namespace InvvardDev.Ifttt.Toolkit.Attributes;

#pragma warning disable CA1018 // Custom attributes should have AttributeUsage attribute: This is a base class for custom attributes and it cannot be instantiated.
public class ProcessorAttributeBase : Attribute
#pragma warning restore CA1018 // Custom attributes should have AttributeUsage attribute
{
    public string Slug { get; }

    protected ProcessorAttributeBase(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentNullException(nameof(slug), "Slug cannot be null or whitespace.");
        }

        Slug = slug;
    }
}