using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit;

public abstract record TriggerResponse([property: JsonIgnore] string Id,
                                       [property: JsonIgnore] DateTimeOffset Timestamp) : BaseResponse
{
    public Meta Meta { get; init; } = new(Id, Timestamp.ToUnixTimeSeconds().ToString());
}

public record Meta(string Id, string Timestamp);
