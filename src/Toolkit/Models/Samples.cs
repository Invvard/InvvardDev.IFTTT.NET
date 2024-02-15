namespace InvvardDev.Ifttt.Toolkit.Models;

public class Samples
{
    public Processors Triggers { get; private set; } = new();

    internal void SkimEmptyProcessors()
    {
        Triggers.RemoveEmptyProcessors();
        if (Triggers.Count == 0)
        {
            Triggers = default!;
        }
    }
}