#if NET6_0 || NET7_0

using System.Text.Json;

namespace InvvardDev.Ifttt.Core.Configuration;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy SnakeCaseLower { get; } = new();

    public override string ConvertName(string name) => name.ToSnakeCase();
}

#endif