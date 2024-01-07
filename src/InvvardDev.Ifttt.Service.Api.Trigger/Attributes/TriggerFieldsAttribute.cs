namespace InvvardDev.Ifttt.Service.Api.Trigger.Attributes;

public class TriggerFieldsAttribute : Attribute
{
    public string TriggerSlug { get; set; }

    public TriggerFieldsAttribute(string triggerSlug)
    {
        if (string.IsNullOrWhiteSpace(triggerSlug))
        {
            throw new ArgumentException("Trigger slug cannot be null or whitespace.", nameof(triggerSlug));
        }

        TriggerSlug = triggerSlug;
    }
}