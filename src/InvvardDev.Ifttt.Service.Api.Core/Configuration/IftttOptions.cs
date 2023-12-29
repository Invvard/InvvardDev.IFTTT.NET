using System.ComponentModel.DataAnnotations;

namespace InvvardDev.Ifttt.Service.Api.Core.Configuration;

public class IftttOptions
{
    public const string DefaultSectionName = nameof(IftttOptions);
    public const string ServiceKeyHeader = "IFTTT-Service-Key";

    [Required]
    public required string ServiceKey { get; set; } = string.Empty;

    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}
