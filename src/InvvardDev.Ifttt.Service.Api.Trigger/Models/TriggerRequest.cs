using System.Text.Json.Serialization;
using InvvardDev.Ifttt.Service.Api.Core.Models;

namespace InvvardDev.Ifttt.Service.Api.Trigger.Models;

public class TriggerRequestBase
{
        [JsonPropertyName("trigger_identity")]
        public string TriggerIdentity { get; set; } = null!;

        //public required T TriggerFields { get; set; }

        public int Limit { get; init; } = 50;

        [JsonPropertyName("ifttt_source")]
        public Source Source { get; set; } = default!;

        public User User { get; set; } = new User("America/Toronto");
}