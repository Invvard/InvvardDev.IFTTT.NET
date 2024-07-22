using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit;

public record TriggerData([property: JsonIgnore] string Id,
                          [property: JsonIgnore] DateTimeOffset Timestamp)
{
    public Meta Meta { get; init; } = new(Id, Timestamp.ToUnixTimeSeconds().ToString());
}
