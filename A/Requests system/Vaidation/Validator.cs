// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public class Validator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IBaseRequest
{
    // TODO: JR - decide how to word this.
    internal protected const string UserIsNull = "User cannot be null.";
    internal protected const string ChannelDoesNotExist = "Channel does not exist.";
    internal protected const string UserIsAlreadyOnATeamResponse = "The user '{0}' is already on a team.";
    internal protected const string UserIsNotOnATeamResponse = "The user '{0}' is not on a team.";
    internal protected const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";

    // CSV
    internal protected const string CsvMediaType = "text/csv";
    internal protected const string NonCsvFileError = "The uploaded file must be a csv.";

    public IDataWorker DataWorker = DataFactory.CreateDataWorker();

    public void UserNotNull(Func<TRequest, User> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void UserNotNull(Func<TRequest, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void TeamExists(Func<TRequest, string> func)
    {
        RuleFor(r => func(r))
            .Must(t => DataWorker.Teams.DoesTeamExist(t))
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));

    }

    public void TeamExists(Func<TRequest, Team> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));
    }

    public void NewTeamName(Func<TRequest, string> func)
    {
        RuleFor(r => func(r))
            .SetValidator(new NewTeamNameValidator<TRequest>(DataWorker));
    }

    public void UserOnATeam(Func<TRequest, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .Must(u => u.IsOnATeam(DataWorker))
            .WithMessage(r => UserIsNotOnATeamResponse.FormatConst(func(r).Username));
    }

    public void UserNotOnATeam(Func<TRequest, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .Must(u => u.IsOnATeam(DataWorker) is false)
            .WithMessage(r => UserIsAlreadyOnATeamResponse.FormatConst(func(r).Username));
    }

    public void UserOnTeam(Func<TRequest, (DiscordUser, string)> func)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<TRequest>(DataWorker, func));
    }

    public void UserOnTeam(Func<TRequest, (DiscordUser, Team)> func)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<TRequest>(DataWorker, func));
    }

    public void ChannelNotNull(Func<TRequest, DiscordChannel> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(ChannelDoesNotExist);
    }

    public void IsCSVFile(Func<TRequest, DiscordAttachment> func)
    {
        RuleFor(r => func(r))
            .Must(a => a.MediaType.StartsWith(CsvMediaType))
            .WithMessage(r => NonCsvFileError);
    }
}