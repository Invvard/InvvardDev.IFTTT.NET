using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit.Models;

public class TopLevelBaseModel
{
    protected static readonly JsonSerializerOptions JsonSerializerOptions
        = new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
              DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
              DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
              PropertyNameCaseInsensitive = true,
          };
}

public class TopLevelMessageModel<T>() : TopLevelBaseModel
    where T : class, new()
{
    public TopLevelMessageModel(T dataPayload) : this()
        => Data = dataPayload;

    public T Data { get; init; } = new();

    public static string Serialize(T data)
        => Serialize(data, JsonSerializerOptions);

    public static string Serialize(T data, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelMessageModel<T>(data), options);

    public static TopLevelMessageModel<T> Deserialize(string json)
        => Deserialize(json, JsonSerializerOptions);

    public static TopLevelMessageModel<T> Deserialize(string json, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TopLevelMessageModel<T>>(json, options) ?? new TopLevelMessageModel<T>(new T());
}

public class TopLevelErrorModel(IList<ErrorMessage> errors) : TopLevelBaseModel
{
    public IList<ErrorMessage> Errors { get; } = errors;

    public static string Serialize(IList<ErrorMessage> errors)
        => Serialize(errors, JsonSerializerOptions);

    public static string Serialize(IList<ErrorMessage> errors, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelErrorModel(errors), options);
}

public record ErrorMessage(string Message);