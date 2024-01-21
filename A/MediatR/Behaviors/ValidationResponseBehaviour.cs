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

// TODO: JR - add a ResponseBehaviour.
public class ValidationResponseBehaviour<TRequest, TArgs> : IPipelineBehavior<TRequest, Result>
    where TRequest : IInteractionRequest<TArgs>
    where TArgs : InteractionCreateEventArgs
{
    private readonly Validator<TRequest> validator;

    public ValidationResponseBehaviour(Validator<TRequest> validator) =>
        this.validator = validator;

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        var result = await next();

        var response = new InteractionMessage(request.InteractionArgs.Interaction)
            .AsEphemeral(true);

        var services = GetMessageServices(request);
        result.Reasons
            .Where(r => r is IDiscordResponse or ValidationError)
            .Where(r => string.IsNullOrEmpty(r.Message) is false)
            .ForEach(r => response.WithContent(r.Message));

        if (string.IsNullOrEmpty(response.Content) is false)
        {
            await services.Send(response);
        }
        return result;
    }

    private static IDiscordInteractionMessagingServices GetMessageServices(TRequest request)
    {
        IDiscordInteractionMessagingServices services = (IDiscordInteractionMessagingServices)General.DI.GetService(typeof(IDiscordInteractionMessagingServices))!;
        services.Initialise(request);
        return services;
    }
}