using System.Text.Json;

namespace InvvardDev.Ifttt.Toolkit.Models;

public class TopLevelMessageModel<T>(T data) : TopLevelBaseModel
    where T : class, new()
{
    public T Data { get; } = data;

    public static string Serialize(T data)
        => Serialize(data, JsonSerializerOptions);

    public static string Serialize(T data, JsonSerializerOptions options)
        => JsonSerializer.Serialize(new TopLevelMessageModel<T>(data), options);

    public static TopLevelMessageModel<T> Deserialize(string json)
        => Deserialize(json, JsonSerializerOptions);
    
    public static TopLevelMessageModel<T> Deserialize(string json, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TopLevelMessageModel<T>>(json, JsonSerializerOptions) ?? new TopLevelMessageModel<T>(new T());
}