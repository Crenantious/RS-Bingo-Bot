// <copyright file="Validator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests.Validation;

using DSharpPlus.Entities;
using FluentValidation;
using MediatR;
using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public class Validator<T> : AbstractValidator<T>
    where T : IBaseRequest
{
    internal protected const string UserIsNull = "User cannot be null";
    internal protected const string UserIsAlreadyOnATeamResponse = "The user '{0}' is already on a team.";
    internal protected const string UserIsNotOnATeamResponse = "The user '{0}' is not on a team.";
    internal protected const string TeamDoesNotExistResponse = "A team with the name '{0}' does not exist.";

    // CSV
    internal protected const string CsvMediaType = "text/csv";
    internal protected const string NonCsvFileError = "The uploaded file must be a csv.";

    public IDataWorker DataWorker = DataFactory.CreateDataWorker();

    public void UserNotNull(Func<T, User> func)
    {
        RuleFor(r => func(r))
            .NotNull()
            .WithMessage(UserIsNull);
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