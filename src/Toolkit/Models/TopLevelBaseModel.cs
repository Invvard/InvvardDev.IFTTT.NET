using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvvardDev.Ifttt.Toolkit.Models;

public class TopLevelBaseModel
{
    protected static readonly JsonSerializerOptions JsonSerializerOptions
        = new()
          {
              PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
              DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
              PropertyNameCaseInsensitive = true
          };
}