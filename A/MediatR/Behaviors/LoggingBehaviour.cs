// <copyright file="LoggingBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Common;
using System.Diagnostics;
using System.Text;

public class LoggingBehaviour<TRequest> : IPipelineBehavior<TRequest, Result>
    where TRequest : IResultBase
{
    private const string BeganHandlingRequest = "Began handling request of type '{0}'. No additional information found.";
    private const string BeganHandlingRequestWithInfo = "Began handling request of type '{0}'.{1}{2}";
    private const string RequestSucceeded = "The request '{0}' was completed successfully after {1} ms.";
    private const string RequestFailed = "The request '{0}' failed after {1} ms with the following errors:";

    private readonly ILogger<LoggingBehaviour<TRequest>> logger;
    private readonly AdditionalLogInfoForRequest additionalRequestInfo;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger, AdditionalLogInfoForRequest additionalRequestInfo)
    {
        this.logger = logger;
        this.additionalRequestInfo = additionalRequestInfo;
    }

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        LogBeginHandling(request);

        Stopwatch stopwatch = Stopwatch.StartNew();
        Result result = await next();
        stopwatch.Stop();

        if (result.IsFailed)
        {
            logger.LogInformation(RequestFailed.FormatConst(request.GetType(), stopwatch.Elapsed.Milliseconds));
            logger.LogError(CompileReasons(result.Errors));
        }
        else
        {
            logger.LogInformation(RequestSucceeded.FormatConst(request.GetType(), stopwatch.Elapsed.Milliseconds));
            logger.LogInformation(CompileReasons(result.Successes));
        }

        return result;
    }

    private void LogBeginHandling(TRequest request)
    {
        string info = additionalRequestInfo.GetInfo(request);
        if (string.IsNullOrEmpty(info))
        {
            logger.LogInformation(BeganHandlingRequest.FormatConst(request.GetType()));
        }
        else
        {
            logger.LogInformation(BeganHandlingRequest.FormatConst(request.GetType(), Environment.NewLine, info));
        }
    }

    private string CompileReasons<T>(List<T> reasons) where T : IReason
    {
        StringBuilder sb = new();
        reasons.ForEach(r => sb.AppendLine($"Message: {r.Message}, Metadata: {r.Metadata}"));
        return sb.ToString();
    }
}