namespace InvvardDev.Ifttt.Toolkit.Attributes;

#pragma warning disable CA1018 // Custom attributes should have AttributeUsage attribute: This is a base class for custom attributes and it cannot be instantiated.
/// <summary>
/// This is the base class for all processor and data field attributes.
/// </summary>
public class ProcessorAttributeBase : Attribute
#pragma warning restore CA1018 // Custom attributes should have AttributeUsage attribute
{
    /// <summary>
    /// Gets the slug of the processor or data field.
    /// </summary>
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