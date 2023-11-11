﻿// <copyright file="RequestsValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using Requests;
using RSBingo_Framework.Interfaces;
using RSBingoBot.DiscordExtensions;

internal static class RequestsValidationUtilities
{
    private const string UserIsNull = "User cannot be null";
    private const string UserIsAlreadyOnATeamResponse = "This user '{0}' is already on a team.";
    private const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";

    public static void DiscordUserNotNull<T>(this AbstractValidator<T> validator)
        where T : IRequestWithDiscordUser
    {
        validator.RuleFor(r => r.DiscordUser)
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public static void UserNotNull<T>(this AbstractValidator<T> validator)
        where T : IRequestWithUser
    {
        validator.RuleFor(r => r.User)
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public static void DiscordUserOnATeam<T>(this AbstractValidator<T> validator, IDataWorker dataWorker)
        where T : IRequestWithDiscordUser
    {
        validator.RuleFor(r => r.DiscordUser)
            .Must(u => u.IsOnATeam(dataWorker))
            .WithMessage(r => UserIsAlreadyOnATeamResponse.FormatConst(r.DiscordUser));
    }

    public static void TeamExists<T>(this AbstractValidator<T> validator, IDataWorker dataWorker)
        where T : IRequestWithTeamName
    {
        validator.RuleFor(r => r.TeamName)
            .Must(n => dataWorker.Teams.DoesTeamExist(n))
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(r.TeamName));
    }
}