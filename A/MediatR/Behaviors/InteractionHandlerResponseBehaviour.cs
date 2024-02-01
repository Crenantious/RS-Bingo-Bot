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
        Result result;
        string error = string.Empty;

        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            error = InternalError.ErrorMessage;
            result = new Result()
                .WithError(new ExceptionError(e))
                .WithError(new InternalError());
        }

        var response = GetResponse(request, typeof(IDiscordResponse));

        TryAddErrorResponse(error, response);
        TrySetDefaultResponse(response);

        await SendResponse(request, response);

        return result;
    }

    private static void TryAddErrorResponse(string error, InteractionMessage response)
    {
        if (string.IsNullOrEmpty(error) is false)
        {
            response.WithContent(error);
        }
    }

    private void TrySetDefaultResponse(InteractionMessage response)
    {
        if (string.IsNullOrEmpty(response.Content) &&
            responseTracker.HasResponse(Interaction) is false)
        {
            response.WithContent(DefaultResponse);
        }
    }
}