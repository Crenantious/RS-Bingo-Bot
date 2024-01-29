// <copyright file="ValidationResponseBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using FluentResults;
using MediatR;

public class InteractionHandlerResponseBehaviour<TRequest> : InteractionResponseBehaviour<TRequest>
    where TRequest : IInteractionRequest
{
    private const string DefaultResponse = "Process completed.";

    private readonly InteractionResponseTracker responseTracker;

    public InteractionHandlerResponseBehaviour(InteractionResponseTracker responseTracker) =>
        this.responseTracker = responseTracker;

    public override async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var result = await next();

        var response = GetResponse(request, typeof(IDiscordResponse));

        if (string.IsNullOrEmpty(response.Content) &&
            responseTracker.HasResponse(Interaction) is false)
        {
            response.WithContent(DefaultResponse);
        }

        await SendResponse(request, response);

        return result;
    }
}