// <copyright file="InteractionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FluentResults;
using FluentValidation;
using RSBingo_Common;
using RSBingo_Framework.Models;

// TODO : JR - remove this when RequestContext gets removed as there will no longer
// be a need to separate the methods from Validator.
public class InteractionValidator<TRequest> : Validator<RequestContext<TRequest, Result>>
    where TRequest : IInteractionRequest
{
    public void DiscordUserNotNull()
    {
        RuleFor(r => GetInteraction(r).User)
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void DiscordUserOnATeam()
    {
        RuleFor(r => GetInteraction(r).User)
            .Must(u => u.IsOnATeam(DataWorker))
            .WithMessage(r => UserIsNotOnATeamResponse.FormatConst(GetInteraction(r).User.Username));
    }

    public void DiscordUserNotOnATeam()
    {
        RuleFor(r => GetInteraction(r).User)
            .Must(u => u.IsOnATeam(DataWorker) is false)
            .WithMessage(r => UserIsAlreadyOnATeamResponse.FormatConst(GetInteraction(r).User.Username));
    }

    public void DiscordUserOnTeam(Func<TRequest, string> func)
    {
        RuleFor(r => GetInteraction(r).User)
            .SetValidator(new UserOnTeamValidator<RequestContext<TRequest, Result>>(
                DataWorker,
                r => (GetInteraction(r).User, func(r.Request))
                ));
    }

    public void DiscordUserOnTeam(Func<TRequest, Team> func)
    {
        RuleFor(r => GetInteraction(r).User)
            .SetValidator(new UserOnTeamValidator<RequestContext<TRequest, Result>>(
                DataWorker,
                r => (GetInteraction(r).User, func(r.Request))
                ));
    }

    private static (DiscordUser, string) GetData(RequestContext<TRequest, Result> r,
        Func<TRequest, DiscordInteraction, (DiscordUser, string)> func) =>
        func(r.Request, GetInteraction(r));

    private static DiscordInteraction GetInteraction(RequestContext<TRequest, Result> request) =>
        request.MetaData.Get<InteractionCreateEventArgs>().Interaction;
}