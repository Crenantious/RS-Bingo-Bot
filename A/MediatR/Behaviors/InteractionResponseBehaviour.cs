// <copyright file="ValidationResponseBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Validation;
using DSharpPlus.EventArgs;
using FluentResults;
using MediatR;
using RSBingo_Common;

public class InteractionResponseBehaviour<TRequest, TArgs> : IPipelineBehavior<TRequest, Result>
    where TRequest : IInteractionRequest<TArgs>
    where TArgs : InteractionCreateEventArgs
{
    private const string DefaultResponse = "Process completed.";

    private readonly Validator<TRequest> validator;
    private readonly RequestsTracker requestsTracker;

    public InteractionResponseBehaviour(Validator<TRequest> validator, RequestsTracker requestsTracker)
    {
        this.validator = validator;
        this.requestsTracker = requestsTracker;
    }

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var result = await next();

        var response = new InteractionMessage(request.InteractionArgs.Interaction)
            .AsEphemeral(true);

        RequestTracker requestTracker = requestsTracker.Trackers[request];
        AddResponses(requestTracker, response);

        if (string.IsNullOrEmpty(response.Content))
        {
            response.WithContent(DefaultResponse);
        }
        await GetMessageServices(request).Send(response);

        return result;
    }

    private void AddResponses(RequestTracker tracker, InteractionMessage response)
    {
        foreach (RequestTracker childTracker in tracker.Trackers)
        {
            AddResponses(childTracker, response);
        }

        tracker.RequestResult.Reasons
            .Where(r => r is IDiscordResponse)
            .Where(r => string.IsNullOrEmpty(r.Message) is false)
            .ForEach(r => response.WithContent(r.Message));
    }

    private static IDiscordInteractionMessagingServices GetMessageServices(TRequest request)
    {
        IDiscordInteractionMessagingServices services = (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
        services.Initialise(request);
        return services;
    }
}