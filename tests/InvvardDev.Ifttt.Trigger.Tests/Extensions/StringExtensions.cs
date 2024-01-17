using Bogus;

namespace InvvardDev.Ifttt.Trigger.Tests;

internal static class StringExtensions
{
    public static string NewName(this string? name, int count = 2)
        => name ?? string.Join('_', new Faker().Random.WordsArray(count));
}