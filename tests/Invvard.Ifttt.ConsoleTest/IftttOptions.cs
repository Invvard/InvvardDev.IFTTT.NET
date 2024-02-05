using System.ComponentModel.DataAnnotations;

namespace Invvard.Ifttt.ConsoleTest;

public class IftttOptions
{
    public const string DefaultSectionName = nameof(IftttOptions);

    [Required]
    public required string ServiceKey { get; init; } = string.Empty;

    public bool BypassServiceKey { get; init; }

    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}
