using InvvardDev.Ifttt.Contracts;

namespace InvvardDev.Ifttt.Services;

internal class TriggerAutoMapperService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<TriggerAutoMapperService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        logger.LogInformation("Auto-mapping trigger is starting.");

        try
        {
            using var scope = serviceScopeFactory.CreateScope();
            
            var triggerMapper = scope.ServiceProvider.GetRequiredService<ITriggerMapper>();

            await triggerMapper.MapTriggerProcessors(stoppingToken);
            await triggerMapper.MapTriggerFields(stoppingToken);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogInformation(ex, "Auto-mapping trigger is cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Auto-mapping trigger is failed.");
        }
        finally
        {
            logger.LogInformation("Auto-mapping trigger is stopped.");
            await StopAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Auto-mapping trigger is stopping.");
        await base.StopAsync(stoppingToken);
    }
}