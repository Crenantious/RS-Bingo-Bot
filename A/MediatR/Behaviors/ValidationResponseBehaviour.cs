// <copyright file="ValidationResponseBehaviour.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using FluentResults;
using MediatR;

public class ValidationResponseBehaviour<TRequest> : InteractionResponseBehaviour<TRequest>
    where TRequest : IInteractionRequest
{
    public override async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var result = await next();

        var response = GetResponse(request, typeof(ValidationError));

        if (string.IsNullOrEmpty(response.Content) is false)
        {
            await SendResponse(request, response);
        }
        return result;
    }
}