using System.Text.Json;

namespace InvvardDev.Ifttt.Toolkit;

public abstract record BaseResponse
{
    public virtual string Serialize() => Serialize(jsonSerializerOptions);
    
    private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

    private string Serialize(JsonSerializerOptions options)
        => JsonSerializer.Serialize(this, GetType(), options);
}
