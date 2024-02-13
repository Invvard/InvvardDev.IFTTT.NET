using InvvardDev.Ifttt.Models.Core;

namespace InvvardDev.Ifttt;

internal static class ProcessorKeyExtensions
{
    public static string GetProcessorKey(this ProcessorKind kind, string slug) => $"{kind}:{slug}";
}