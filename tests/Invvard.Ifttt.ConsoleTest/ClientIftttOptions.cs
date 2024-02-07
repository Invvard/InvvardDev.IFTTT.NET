using System.ComponentModel.DataAnnotations;

namespace Invvard.Ifttt.ConsoleTest;

public class ClientIftttOptions
{
    public const string DefaultSectionName = nameof(ClientIftttOptions);

    [Required]
    public required string ServiceKey { get; init; } = string.Empty;

    public bool BypassServiceKey { get; init; }
}
