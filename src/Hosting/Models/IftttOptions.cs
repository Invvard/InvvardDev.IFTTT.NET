namespace InvvardDev.Ifttt.Hosting.Models;

public class IftttOptions
{
    /// <summary>
    /// Gets or sets he IFTTT service key.
    /// </summary>
    public string? ServiceKey { get; set; }

    /// <summary>
    /// Gets or sets the base address for the IFTTT Realtime API. Default: https://realtime.ifttt.com/v1/notifications/.
    /// </summary>
    public string RealTimeBaseAddress { get; set; } = "https://realtime.ifttt.com/v1/notifications/";
}