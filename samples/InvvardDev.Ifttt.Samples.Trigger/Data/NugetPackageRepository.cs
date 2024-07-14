using System.Text.Json;
using InvvardDev.Ifttt.Samples.Trigger.Data.Models;

namespace InvvardDev.Ifttt.Samples.Trigger.Data;

public class NugetPackageRepository : IDataRepository<NugetPackageVersion>
{
    private readonly List<NugetPackageVersion> nugetPackageVersions = [];

    public NugetPackageRepository()
    {
        LoadData().GetAwaiter().GetResult();
    }

    private async Task LoadData()
    {
        var jsonString = await File.ReadAllTextAsync("Resources/nuget_package_db.json");

        if (JsonSerializer.Deserialize<List<NugetPackageVersion>>(jsonString) is { Count: > 0 } data)
        {
            nugetPackageVersions.AddRange(data);
        }
    }

    public Task<IReadOnlyCollection<NugetPackageVersion>> GetAll(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyCollection<NugetPackageVersion>>(nugetPackageVersions);

    public Task<NugetPackageVersion?> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var nugetPackageVersion = nugetPackageVersions.Find(x => x.PackageName == name);
        
        return Task.FromResult(nugetPackageVersion);
    }

    public Task Add(NugetPackageVersion data, CancellationToken cancellationToken = default)
    {
        nugetPackageVersions.Add(data);

        return Task.CompletedTask;
    }
}
