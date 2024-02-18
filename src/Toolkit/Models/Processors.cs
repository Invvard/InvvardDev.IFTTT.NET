namespace InvvardDev.Ifttt.Toolkit.Models;

public class Processors : Dictionary<string, Dictionary<string, string>>
{
    public Processors AddProcessor(string slug)
    {
        Add(slug, new Dictionary<string, string>());

        return this;
    }

    public Processors AddDataField(string slug, string dataFieldSlug, string data)
    {
        if (TryGetValue(slug, out var dataFields))
        {
            dataFields.Add(dataFieldSlug, data);
        }

        return this;
    }
    
    internal void RemoveEmptyProcessors()
    {
        var emptyProcessors = this.Where(p => p.Value.Count == 0).ToList();
        foreach (var emptyProcessor in emptyProcessors)
        {
            Remove(emptyProcessor.Key);
        }
    }
}