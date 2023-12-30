using System.Text.Json;

namespace InvvardDev.Ifttt.Service.Api.Core.Models;

public record TopLevelErrorModel(IList<ErrorMessage> Errors) : TopLevelBaseModel
{
    public static string Serialize(IList<ErrorMessage> errors)
        => Serialize(errors, JsonSerializerOptions);

    public static string Serialize(IList<ErrorMessage> errors, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelErrorModel(errors), options);
}

public record ErrorMessage(string Message);
