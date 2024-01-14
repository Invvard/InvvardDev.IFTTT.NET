using System.Text.Json.Serialization;
using InvvardDev.Ifttt.Service.Api.Core.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Models;

public class TriggerRequest
{
    public string? TriggerIdentity { get; set; }

    public Dictionary<string, string> TriggerFields { get; set; } = new();

    public int Limit { get; init; } = 50;

    [JsonPropertyName("ifttt_source")]
    public Source Source { get; set; } = default!;

    public User User { get; set; } = new();
}
