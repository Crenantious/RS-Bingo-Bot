// <copyright file="RequestTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;
using RSBingo_Common;
using System.Text;

// TODO: JR use a semaphore to ensure the ids are unique.
public class RequestTracker
{
    private const string RequestNotCompletedError = "The request failed to complete, this is an internal error.";

    private static RequestsTracker requestsTracker;
    private static int CurrentRequestId = 0;

    private StringBuilder logInfo = new();
    private List<RequestTracker> trackers = new();

    public IBaseRequest Request { get; }
    public IBaseRequest? ParentRequest { get; }
    public int RequestId { get; }
    public DateTime CreationTimeStamp { get; }
    public DateTime CompletionTimeStamp { get; private set; }
    public IResultBase RequestResult { get; set; } = Result.Fail(RequestNotCompletedError);
    public IReadOnlyList<RequestTracker> Trackers { get; }

    static RequestTracker()
    {
        requestsTracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    public RequestTracker(IBaseRequest request, IBaseRequest? parentRequest)
    {
        this.Request = request;
        this.ParentRequest = parentRequest;

        RequestId = CurrentRequestId++;
        Trackers = trackers.AsReadOnly();
        CreationTimeStamp = DateTime.UtcNow;

        if (parentRequest is not null)
        {
            requestsTracker.Trackers[parentRequest].trackers.Add(this);
        }
    }

    public void Completed(IResultBase result)
    {
        RequestResult = result;
        CompletionTimeStamp = DateTime.UtcNow;
    }
}