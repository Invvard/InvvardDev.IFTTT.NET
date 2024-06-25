using Microsoft.Extensions.Logging;
using Moq;

namespace InvvardDev.Ifttt.TestFactories.Utilities;

public static class LoggerMockExtensions
{
    public static void VerifyInformationContains<T>(this Mock<ILogger<T>> logger, string contained, Func<Times> times)
        => logger.VerifyLoggerContains(LogLevel.Information, contained, times());

    public static void VerifyInformationContains<T>(this Mock<ILogger<T>> logger, string contained, Times times)
        => logger.VerifyLoggerContains(LogLevel.Information, contained, times);

    public static void VerifyInformationContains<T>(this ILogger<T> logger, string contained, Func<Times> times)
        => Mock.Get(logger)
               .VerifyLoggerContains(LogLevel.Information, contained, times());

    public static void VerifyWarningContains<T>(this ILogger<T> logger, string contained, Func<Times> times)
        => Mock.Get(logger)
               .VerifyLoggerContains(LogLevel.Warning, contained, times());

    public static void VerifyWarningContains<TException>(this ILogger logger, string contained, Func<Times> times)
        where TException : Exception
        => Mock.Get(logger)
               .VerifyLoggerContains<TException>(LogLevel.Warning, contained, times());

    // exception is only there to capture generic type when invoking
    public static void VerifyWarningContains<TException>(this ILogger logger, string contained, Func<Times> times, TException ex)
        where TException : Exception
        => Mock.Get(logger)
               .VerifyLoggerContains<TException>(LogLevel.Warning, contained, times());

    // exception is only there to capture generic type when invoking
    public static void VerifyErrorContains<TException>(this ILogger logger, string contained, string exceptionMessage, Func<Times> times)
        where TException : Exception
        => Mock.Get(logger)
               .VerifyLoggerContains<TException>(LogLevel.Error, contained, exceptionMessage, times());

    public static void VerifyErrorContains<TException>(this ILogger logger, string contained, Func<Times> times)
        where TException : Exception
        => Mock.Get(logger)
               .VerifyLoggerContains<TException>(LogLevel.Error, contained, times());

    // exception is only there to capture generic type when invoking
    public static void VerifyCriticalContains<TException>(this ILogger logger, string contained, TException ex, Func<Times> times)
        where TException : Exception
        => logger.VerifyCriticalContains<TException>(contained, times);

    public static void VerifyCriticalContains<TException>(this ILogger logger, string contained, Func<Times> times)
        where TException : Exception
        => Mock.Get(logger)
               .VerifyLoggerContains<TException>(LogLevel.Critical, contained, times());

    private static void VerifyLoggerContains<T>(this Mock<ILogger<T>> logger, LogLevel logLevel, string contained, Times times)
        => logger.Verify(x => x.Log(It.Is<LogLevel>(l => l == logLevel),
                                    It.IsAny<EventId>(),
                                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(contained)),
                                    It.IsAny<Exception>(),
                                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                         times);

    private static void VerifyLoggerContains<TException>(this Mock<ILogger> logger, LogLevel logLevel, string contained, Times times)
        where TException : Exception
        => logger.Verify(x => x.Log(It.Is<LogLevel>(l => l == logLevel),
                                    It.IsAny<EventId>(),
                                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(contained)),
                                    It.IsAny<TException>(),
                                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                         times);
    
    private static void VerifyLoggerContains<TException>(this Mock<ILogger> logger, LogLevel logLevel, string contained, string exceptionMessage, Times times)
        where TException : Exception
        => logger.Verify(x => x.Log(It.Is<LogLevel>(l => l == logLevel),
                                    It.IsAny<EventId>(),
                                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(contained)),
                                    It.Is<TException>(ex => ex.Message == exceptionMessage),
                                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                         times);
}
