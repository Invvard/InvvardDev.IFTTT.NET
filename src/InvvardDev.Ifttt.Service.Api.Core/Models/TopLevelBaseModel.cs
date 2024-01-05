using System.Text.Json.Serialization;
using System.Text.Json;

namespace InvvardDev.Ifttt.Service.Api.Core.Models;

public class TopLevelBaseModel
{
    protected static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };
}
