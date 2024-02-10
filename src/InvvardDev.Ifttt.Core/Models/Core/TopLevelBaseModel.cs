using System.Text.Json;
using System.Text.Json.Serialization;
using InvvardDev.Ifttt.Shared.Configuration;
#if NET6_0 || NET7_0
#endif

namespace InvvardDev.Ifttt.Shared.Models;

public class TopLevelBaseModel
{
    protected static readonly JsonSerializerOptions JsonSerializerOptions
        = new()
          {
#if NET6_0 || NET7_0
              PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCaseLower,
#elif NET8_0_OR_GREATER
              PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
#endif
              DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
              PropertyNameCaseInsensitive = true
          };
}