using System.Text.Json.Serialization;
using InvvardDev.Ifttt.Models.Core;

namespace InvvardDev.Ifttt.Toolkit;

/// <summary>
/// Model to hold the trigger request data received from IFTTT.
/// </summary>
public class TriggerRequest
{
    /// <summary>
    /// Gets or sets unique identifier for this set of <see cref="TriggerFields"/>.
    /// </summary>
    public string? TriggerIdentity { get; set; }

    /// <summary>
    /// Gets or sets the map of trigger field slugs to values.
    /// The key is the trigger field slug and value the parameter set by the user.
    /// </summary>
    public Dictionary<string, string> TriggerFields { get; set; } = new();

    /// <summary>
    /// Gets or sets the maximum number of items to return. Default is 50.
    /// </summary>
    public int Limit { get; init; } = 50;

    /// <summary>
    /// Gets or sets information about the personal Applet on IFTTT that triggered this request.
    /// </summary>
    [JsonPropertyName("ifttt_source")]
    public Source Source { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user information related to this request.
    /// </summary>
    public User User { get; set; } = new();
}