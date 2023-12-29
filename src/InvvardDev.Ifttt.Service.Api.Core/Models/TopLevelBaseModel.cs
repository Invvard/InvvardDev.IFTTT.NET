using System.Text.Json.Serialization;
using System.Text.Json;

namespace InvvardDev.Ifttt.Service.Api.Core.Models;

public record TopLevelBaseModel
{
    protected static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull,
    };
}
