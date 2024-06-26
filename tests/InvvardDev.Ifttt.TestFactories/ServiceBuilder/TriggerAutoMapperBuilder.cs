using InvvardDev.Ifttt.Contracts;
using InvvardDev.Ifttt.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

// ReSharper disable ParameterHidesMember

namespace InvvardDev.Ifttt.TestFactories.ServiceBuilder;

internal class TriggerAutoMapperBuilder
{
    private IHostedService? hostedService;
    private ILogger<TriggerAutoMapperService>? logger;
    private IServiceScopeFactory? serviceScopeFactory;
    private ITriggerMapper? triggerMapper;
    private CancellationTokenSource? cancellationTokenSource;
    
    public TriggerAutoMapperBuilder WithCancellationTokenSource(CancellationTokenSource cts)
    {
        cancellationTokenSource = cts;
        return this;
    }
    
    public TriggerAutoMapperBuilder WithLogger(ILogger<TriggerAutoMapperService> logger)
    {
        this.logger = logger;
        return this;
    }

    public TriggerAutoMapperBuilder WithTriggerMapper(ITriggerMapper mapper)
    {
        triggerMapper = mapper;
        return this;
    }
    
    public IHostedService Build()
    {
        cancellationTokenSource ??= new CancellationTokenSource();
        logger ??= Mock.Of<ILogger<TriggerAutoMapperService>>();
        serviceScopeFactory ??= Mock.Of<IServiceScopeFactory>();
        triggerMapper ??= Mock.Of<ITriggerMapper>();
        
        IServiceCollection services = new ServiceCollection();
        
        services.AddSingleton<IHostedService, TriggerAutoMapperService>();
        services.AddSingleton(logger);
        services.AddSingleton(serviceScopeFactory);
        services.AddSingleton(triggerMapper);

        hostedService = services.BuildServiceProvider().GetRequiredService<IHostedService>();

        return hostedService as TriggerAutoMapperService ?? throw new InvalidOperationException($"Hosted service is not of type {nameof(TriggerAutoMapperService)}.");
    }
}
