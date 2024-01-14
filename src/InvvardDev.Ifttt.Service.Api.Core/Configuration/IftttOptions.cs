using System.ComponentModel.DataAnnotations;

namespace InvvardDev.Ifttt.Service.Api.Core.Configuration;

public class IftttOptions
{
    public const string DefaultSectionName = nameof(IftttOptions);

    [Required]
    public required string ServiceKey { get; init; } = string.Empty;

    public bool BypassServiceKey { get; init; } = false;

    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}
