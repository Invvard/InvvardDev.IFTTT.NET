using System.Text.Json.Serialization;
using InvvardDev.Ifttt.Shared;
using InvvardDev.Ifttt.Shared.Models;

namespace InvvardDev.Ifttt.Trigger.Models;

public class TriggerRequest
{
    public string? TriggerIdentity { get; set; }

    public Dictionary<string, string> TriggerFields { get; set; } = new();

    public int Limit { get; init; } = 50;

    [JsonPropertyName("ifttt_source")]
    public Source Source { get; set; } = default!;

    public User User { get; set; } = new();
}
