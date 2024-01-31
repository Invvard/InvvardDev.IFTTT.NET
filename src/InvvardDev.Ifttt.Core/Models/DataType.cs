namespace InvvardDev.Ifttt.Core.Models;

public record DataType(string Slug, Type ProcessorType)
{
    public Type? DataFieldsType { get; init; }
}