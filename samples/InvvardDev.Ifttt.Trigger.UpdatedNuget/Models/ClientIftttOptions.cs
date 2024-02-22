using System.ComponentModel.DataAnnotations;

namespace InvvardDev.Ifttt.Trigger.UpdatedNuget.Models;

public class ClientIftttOptions
{
    public const string DefaultSectionName = nameof(ClientIftttOptions);

    [Required]
    public required string ServiceKey { get; init; } = string.Empty;

    public bool BypassServiceKey { get; init; }
}