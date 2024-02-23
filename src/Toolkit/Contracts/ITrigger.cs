﻿using InvvardDev.Ifttt.Toolkit.Models;

namespace InvvardDev.Ifttt.Toolkit.Contracts;

/// <summary>
/// This interface defines the method signatures that will be called when the trigger endpoint is hit.
/// </summary>
public interface ITrigger
{
    /// <summary>
    /// This method is called after the trigger endpoint has been hit.
    /// It receives a <see cref="TriggerRequest"/> object that contains the trigger fields data.
    /// It should hold the logic to execute the trigger.
    /// </summary>
    /// <param name="triggerRequest">The trigger request that contains the trigger fields data.</param>
    /// <param name="cancellationToken"></param>
    Task ExecuteAsync(TriggerRequest triggerRequest, CancellationToken cancellationToken = default);
}