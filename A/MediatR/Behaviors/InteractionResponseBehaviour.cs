// <copyright file="ValidationResponseBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using MediatR;
using RSBingo_Common;
using System.Threading;

public abstract class InteractionResponseBehaviour<TRequest> : IPipelineBehavior<TRequest, Result>
    where TRequest : IBaseRequest, IRequestResponse
{
    private readonly InteractionMessageFactory interactionMessageFactory;

    private bool hasInternalError = false;

    public InteractionResponseBehaviour()
    {
        this.interactionMessageFactory = General.DI.GetService<InteractionMessageFactory>();
    }

    public abstract Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken);

    protected Message GetResponse(TRequest request, params Type[] responseTypes)
    {
        Message response = GetResponseMessage(interactionMessageFactory, request);

        RequestTracker requestTracker = request.GetTracker();
        AddResponses(requestTracker, response, responseTypes);

        return response;
    }

    // TODO: JR - make better. Maybe make a class that has a static method Create<T>(Func<T, Message> getMessage).
    // Loop through a list of these and return on first match.
    public static Message GetResponseMessage(InteractionMessageFactory interactionMessageFactory, IRequestResponse request)
    {
        if (request is IInteractionResponseOverride)
        {
            return ((IInteractionResponseOverride)request).ResponseOverride;
        }

        if (request is IInteractionRequest)
        {
            return interactionMessageFactory.Create(((IInteractionRequest)request).GetDiscordInteraction())
                .AsEphemeral(true);
        }

        throw new NotSupportedException("The request does not have an associated interaction to respond to.");
    }

    private void AddResponses(RequestTracker tracker, Message response, params Type[] responseTypes)
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

    private void CheckInternalError(RequestTracker tracker, Message response)
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

    protected async Task<Result> SendResponse(TRequest request, Message response)
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