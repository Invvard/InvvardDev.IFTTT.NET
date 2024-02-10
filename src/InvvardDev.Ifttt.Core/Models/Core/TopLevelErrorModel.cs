using System.Text.Json;

namespace InvvardDev.Ifttt.Shared.Models;

public class TopLevelErrorModel(IList<ErrorMessage> Errors) : TopLevelBaseModel
{
    public static string Serialize(IList<ErrorMessage> errors)
        => Serialize(errors, JsonSerializerOptions);

    public static string Serialize(IList<ErrorMessage> errors, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelErrorModel(errors), options);
}

public record ErrorMessage(string Message);
