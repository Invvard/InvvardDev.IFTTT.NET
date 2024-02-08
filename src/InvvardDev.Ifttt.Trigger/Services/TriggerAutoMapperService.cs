using InvvardDev.Ifttt.Trigger.Contracts;

namespace InvvardDev.Ifttt.Trigger.Services;

public class TriggerAutoMapperService(ILogger<TriggerAutoMapperService> logger, ITriggerMapper triggerMapper) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        logger.LogInformation("Auto-mapping trigger is starting.");

        try
        {
            var processorMapperTask = Task.Run(triggerMapper.MapTriggerProcessors, stoppingToken);
            var fieldsMapperTask = Task.Run(triggerMapper.MapTriggerFields, stoppingToken);

            await Task.WhenAll(processorMapperTask, fieldsMapperTask);
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
        }
    }
}