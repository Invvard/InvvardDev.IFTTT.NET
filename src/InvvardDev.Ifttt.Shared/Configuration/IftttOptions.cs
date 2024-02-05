using System.ComponentModel.DataAnnotations;

namespace InvvardDev.Ifttt.Shared.Configuration;

public class IftttOptions
{
    public const string DefaultSectionName = nameof(IftttOptions);

    [Required]
#if NET7_0_OR_GREATER
    public required string ServiceKey { get; init; } = string.Empty;
#else
    public string ServiceKey { get; init; } = string.Empty;
#endif

    public bool BypassServiceKey { get; init; } = false;

    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}
