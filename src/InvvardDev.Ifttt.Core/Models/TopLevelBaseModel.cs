using System.Text.Json;
using System.Text.Json.Serialization;
#if NET6_0 || NET7_0
using InvvardDev.Ifttt.Core.Configuration;
#endif

namespace InvvardDev.Ifttt.Core.Models;

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