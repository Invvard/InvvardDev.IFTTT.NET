using System.Text.Json;

namespace InvvardDev.Ifttt.Service.Api.Core.Models;

public record TopLevelMessageModel(object Data) : TopLevelBaseModel
{
    public static string Serialize(object data)
        => Serialize(data, JsonSerializerOptions);

    public static string Serialize(object data, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelMessageModel(data), options);
}
