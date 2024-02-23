namespace InvvardDev.Ifttt.Toolkit.Models;

/// <summary>
/// Base class for trigger fields.
/// </summary>
public class TriggerFieldsBase
{
    public Dictionary<string, string> Metadata { get; } = new();
}