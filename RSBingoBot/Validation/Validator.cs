// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.DiscordExtensions;

internal class Validator<T> : AbstractValidator<T>
    where T : IBaseRequest
{
    private const string UserIsNull = "User cannot be null";
    private const string UserIsAlreadyOnATeamResponse = "The user '{0}' is already on a team.";
    private const string UserIsNotOnATeamResponse = "The user '{0}' is not on a team.";
    private const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";

    // CSV
    private const string CsvMediaType = "text/csv";
    private const string NonCsvFileError = "The uploaded file must be a csv.";

    public IDataWorker DataWorker = DataFactory.CreateDataWorker();

    public void UserNotNull(Func<T, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void UserNotNull(Func<T, User> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
    }

    public void UserOnATeam(Func<T, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .Must(u => u.IsOnATeam(DataWorker))
            .WithMessage(r => UserIsNotOnATeamResponse.FormatConst(func(r)));
    }

    public void UserOnTeam(Func<T, (DiscordUser, string)> func)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<T>(DataWorker, func));
    }

    public void UserOnTeam(Func<T, (DiscordUser, Team)> func)
    {
        RuleFor(r => func(r).Item1)
            .SetValidator(new UserOnTeamValidator<T>(DataWorker, func));
    }

    public void UserNotOnATeam(Func<T, DiscordUser> func)
    {
        RuleFor(r => func(r))
            .Must(u => u.IsOnATeam(DataWorker) is false)
            .WithMessage(r => UserIsAlreadyOnATeamResponse.FormatConst(func(r)));
    }

    public void TeamExists(Func<T, string> func)
    {
        RuleFor(r => func(r))
            .Must(t => DataWorker.Teams.DoesTeamExist(t))
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));

    }

    public void TeamExists(Func<T, Team> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(r => TeamDoesNotExistResponse.FormatConst(func(r)));
    }

    public void NewTeamName(Func<T, string> func)
    {
        RuleFor(r => func(r))
            .SetValidator(new NewTeamNameValidator<T>(DataWorker));
    }

    public void IsCSVFile(Func<T, DiscordAttachment> func)
    {
        RuleFor(r => func(r))
            .Must(a => a.MediaType.StartsWith(CsvMediaType))
            .WithMessage(r => NonCsvFileError);
    }
}