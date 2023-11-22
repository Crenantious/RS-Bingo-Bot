// <copyright file="LoggingBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using MediatR;
using FluentResults;
using System.Diagnostics;
using DiscordLibrary.Interfaces;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, Result>
    where TRequest : IValidatable<TResult>
{
    private const string BeganHandlingRequest = "Began handling request '{0}'.";
    private const string RequestSucceeded = "The request '{0}' was completed successfully after {1} ms.";
    private const string RequestFailed = "The request '{0}' failed after {1} ms with the following errors:";

    private readonly ILogger<LoggingBehaviour<TRequest, TResult>> logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResult>> logger) =>
        this.logger = logger;

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        logger.LogInformation(BeganHandlingRequest.FormatConst(request.GetType()));

        Stopwatch stopwatch = Stopwatch.StartNew();
        Result result = await next();
        stopwatch.Stop();

        if (result.IsFailed)
        {
            logger.LogInformation(RequestFailed.FormatConst(request.GetType(), stopwatch.Elapsed.Milliseconds));
            LogErrors(result);
        }
        else
        {
            logger.LogInformation(RequestSucceeded.FormatConst(request.GetType(), stopwatch.Elapsed.Milliseconds));
        }

        return result;
    }

    private void LogErrors(Result result)
    {
        foreach (IError error in result.Errors)
        {
            logger.LogError(error.Message, error.Metadata);
        }
    }
}