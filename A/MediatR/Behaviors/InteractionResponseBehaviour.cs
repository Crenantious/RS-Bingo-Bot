// <copyright file="ValidationResponseBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;
using FluentResults;
using MediatR;
using RSBingo_Common;
using System.Threading;

public abstract class InteractionResponseBehaviour<TRequest> : IPipelineBehavior<TRequest, Result>
    where TRequest : IInteractionRequest
{
    private readonly RequestsTracker requestsTracker;

    private bool hasInternalError = false;

    protected DiscordInteraction Interaction { get; private set; } = null!;

    public InteractionResponseBehaviour()
    {
        this.requestsTracker = (RequestsTracker)General.DI.GetService(typeof(RequestsTracker))!;
    }

    public abstract Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken);

    protected InteractionMessage GetResponse(TRequest request, params Type[] responseTypes)
    {
        Interaction = request.GetDiscordInteraction();

        var response = new InteractionMessage(Interaction)
            .AsEphemeral(true);

        RequestTracker requestTracker = request.GetTracker();
        AddResponses(requestTracker, response, responseTypes);

        return response;
    }

    private void AddResponses(RequestTracker tracker, InteractionMessage response, params Type[] responseTypes)
    {
        foreach (RequestTracker childTracker in tracker.Trackers)
        {
            AddResponses(childTracker, response, responseTypes);
        }

        CheckInternalError(tracker, response);

        tracker.RequestResult.Reasons
            .Where(r => DoesInherit(r, responseTypes))
            .Where(r => string.IsNullOrEmpty(r.Message) is false)
            .Where(r => r is not InternalError)
            .ForEach(r => response.WithContent(r.Message));
    }

    private void CheckInternalError(RequestTracker tracker, InteractionMessage response)
    {
        if (hasInternalError)
        {
            // The user only needs to be sent one message stating an internal error even if there's multiple.
            return;
        }

        var internalError = tracker.RequestResult.Reasons.FirstOrDefault(r => r is InternalError);
        if (internalError != default)
        {
            hasInternalError = true;
            response.WithContent(internalError.Message);
        }
    }

    private static bool DoesInherit(IReason r, Type[] responseTypes) =>
        responseTypes.Any(t => t.IsAssignableFrom(r.GetType()));

    protected async Task<Result> SendResponse(TRequest request, InteractionMessage response)
    {
        if (string.IsNullOrWhiteSpace(response.Content))
        {
            return new Result();
        }

        return await GetMessageServices(request).SendRequestResultsResponse(response);
    }

    private static IBehaviourServices GetMessageServices(TRequest request)
    {
        IBehaviourServices services = (IBehaviourServices)General.DI.GetService(typeof(IBehaviourServices))!;
        services.Initialise(request);
        return services;
    }
}