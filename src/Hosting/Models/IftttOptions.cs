namespace InvvardDev.Ifttt.Hosting.Models;

public class IftttOptions
{
    public string? ServiceKey { get; set; }

    /// <summary>
    /// Gets or sets the base address for the IFTTT Realtime API (default: https://realtime.ifttt.com/v1/notifications/).
    /// </summary>
    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}