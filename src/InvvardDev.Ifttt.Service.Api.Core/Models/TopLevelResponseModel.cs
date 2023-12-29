using System.Text.Json;

namespace InvvardDev.Ifttt.Service.Api.Core.Models;

public record TopLevelResponseModel(object Data) : TopLevelBaseModel
{
    public static string Serialize(object data)
        => Serialize(data, jsonSerializerOptions);

    public static string Serialize(object data, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelResponseModel(data), options);
}
