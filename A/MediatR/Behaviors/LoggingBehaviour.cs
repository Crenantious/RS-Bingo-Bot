// <copyright file="LoggingBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using RSBingo_Common;
using System.Text;

// TODO: JR - add RequestType to requests (Command, UserEndpoint, Database, Discord etc.) and colour code
// logging based on them to make it easier to read where each request section lies.
public class LoggingBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private const string LogPrefix = "----Begin request chain----";
    private const string LogSuffix = "----End request chain----";
    private const string BeganHandlingRequest = "Began handling request of type '{0}'.";
    private const string BeganHandlingRequestWithInfo = "Began handling request of type '{0}'.{1}{2}";
    private const string RequestSucceeded = "The request '{0}' was completed successfully after {1} ms.";
    private const string RequestFailed = "The request '{0}' failed after {1} ms with the following errors:";

    private readonly ILogger<LoggingBehaviour<TRequest, TResult>> logger;
    private readonly RequestLogInfo<TResult> additionalRequestInfo;
    private readonly RequestsTracker requestsTracker;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResult>> logger, RequestLogInfo<TResult> additionalRequestInfo,
        RequestsTracker requestsTracker)
    {
        this.logger = logger;
        this.additionalRequestInfo = additionalRequestInfo;
        this.requestsTracker = requestsTracker;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        TResult result = await next();
        var tracker = requestsTracker.Trackers[request];
        tracker.Completed(result);

        // Only log the top level request since all its children get recursively logged here.
        if (tracker.ParentRequest is null)
        {
            StringBuilder info = new();
            info.AppendLine(LogPrefix);
            AppendTrackerInfo(tracker, info);
            info.AppendLine(LogSuffix);
            logger.LogInformation(info.ToString());
        }

        return result;
    }

    private void AppendTrackerInfo(RequestTracker tracker, StringBuilder info)
    {
        string parent = tracker.ParentRequest is null ? "None" :
                        requestsTracker.Trackers[tracker.ParentRequest].RequestId.ToString();

        info.AppendLine(
            $"Request: {tracker.Request.GetType()}, " +
            $"id: {tracker.RequestId}, " +
            $"parent id: {parent}, " +
            $"created: {tracker.CreationTimeStamp}, " +
            $"finished: {tracker.CompletionTimeStamp}, " +
            $"elapsed: {(tracker.CompletionTimeStamp - tracker.CreationTimeStamp).Milliseconds}ms, " +
            $"success: {tracker.RequestResult.IsSuccess}, " +
            $"successes: {{ {string.Join(", ", tracker.RequestResult.Successes.Select(s => s.Message))} }}, " +
            $"errors: {{ {string.Join(", ", tracker.RequestResult.Errors.Select(e => e.Message))} }}.");

        tracker.Trackers.ForEach(t => AppendTrackerInfo(t, info));
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